{{/*
Nombre base del chart
*/}}
{{- define "accounts-api.name" -}}
{{- .Chart.Name | trunc 63 | trimSuffix "-" -}}
{{- end -}}

{{/*
Etiquetas comunes
*/}}
{{- define "accounts-api.labels" -}}
app.kubernetes.io/name: {{ include "accounts-api.name" . }}
app.kubernetes.io/instance: {{ .Release.Name }}
app.kubernetes.io/managed-by: {{ .Release.Service }}
helm.sh/chart: {{ .Chart.Name }}-{{ .Chart.Version }}
{{- end -}}
