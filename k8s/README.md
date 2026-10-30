# Kubernetes layout

Shared Deployment/Service: `k8s/base/`. Environment overlays: `k8s/overlays/dev/`, `qa/`, and `prod/`.

Each overlay expects an existing Kubernetes Secret named `flashcardapp-db-secret-dev`, `flashcardapp-db-secret-qa`, or `flashcardapp-db-secret-prod` with a `connection-string` key.

Commit only the `secret.example.yaml` templates; provision actual Secrets securely in the cluster. For example (in the correct namespace):

```sh
kubectl create secret generic flashcardapp-db-secret-dev --from-literal=connection-string="YOUR_DB_CONNECTION_STRING"
kubectl apply -k k8s/overlays/dev
```

The examples are not automatically applied by Kustomize. Use distinct credentials for each environment.
