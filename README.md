# Dapr .NET SDK + Redis State Store + Jaeger Tracing

This sample demonstrates a .NET Web API using the Dapr .NET SDK to interact with a Redis-backed Dapr state store. It also shows how to trace these operations using Jaeger via the OpenTelemetry (OTLP gRPC) exporter.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Dapr CLI](https://docs.dapr.io/getting-started/install-dapr/)
- [Docker](https://www.docker.com/)

## Start Jaeger Locally

Run Jaeger with OTLP gRPC support:

```bash
docker run -d --name jaeger \
  -e COLLECTOR_OTLP_ENABLED=true \
  -p 16686:16686 \
  -p 4317:4317 \
  jaegertracing/all-in-one:1.53
```

- Jaeger UI: http://localhost:16686
- OTLP gRPC: `localhost:4317`

## Configure Dapr Tracing

Create a `dapr-config.yaml`:

```yaml
apiVersion: dapr.io/v1alpha1
kind: Configuration
metadata:
  name: tracing
spec:
  tracing:
    samplingRate: "1"
    stdout: true
    otel:
      endpointAddress: "localhost:4317"
      isSecure: false
      protocol: grpc
```

## Create Redis State Store Component

Create `.dapr/components/statestore.yaml`:

```yaml
apiVersion: dapr.io/v1alpha1
kind: Component
metadata:
  name: statestore
spec:
  type: state.redis
  version: v1
  metadata:
    - name: redisHost
      value: localhost:6379
    - name: redisPassword
      value: ""
```

> Make sure Redis is running locally on port 6379.

## Run the App with Dapr

```bash
dapr run \
  --app-id myservice \
  --app-port 5000 \
  --dapr-http-port 3500 \
  --resources-path ./components \
  --config ./dapr-config.yaml \
  -- dotnet run --project MyService
```

## Test Service Invocationvia Dapr API

In a new tab:

```bash
curl -X POST http://localhost:3500/v1.0/state/statestore -H "Content-Type: application/json" -d '[{"key":"foo","value":"bar"}]'
```

## Test State Store via Dapr SDK

Store a value:

```bash
curl -X POST http://localhost:5000/state/mykey \
  -H "Content-Type: application/json" \
  -d '{ "keys": ["order_1", "order_2"] }'
```

Get the value:

```bash
curl http://localhost:5000/state/mykey
```

## Test State Store via Dapr APIs

Store a value:

```bash
curl -X POST http://localhost:3500/v1.0/state/statestore -H "Content-Type: application/json" -d '[{"key":"foo","value":"bar"}]'
```

Get the value:

```bash
curl http://localhost:3500/v1.0/state/statestore/foo
```

## View Traces in Jaeger

Go to: [http://localhost:16686](http://localhost:16686)

- Search for the service: `myservice`
- You should see traced spans like:
  - `myservice: CallLocal/myservice/hello`
  - `myservice: /v1.0/state/statestore`

> Uncomment the code in Program.cs to see the traces for Dapr SDK calls as the statestore calls are failing as-is.
