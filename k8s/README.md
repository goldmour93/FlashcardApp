# Kubernetes configuration layout

This project follows a single Kubernetes config pattern with environment-specific files:

- `k8s/dev/configmap-dev.yaml`
- `k8s/dev/secret-dev.yaml`
- `k8s/qa/configmap-qa.yaml`
- `k8s/qa/secret-qa.yaml`
- `k8s/prod/configmap-prod.yaml`
- `k8s/prod/secret-prod.yaml`

The naming convention clearly identifies the environment and the resource type, which keeps config values easier to reason about and avoids mixing dev/qa/prod settings together.

