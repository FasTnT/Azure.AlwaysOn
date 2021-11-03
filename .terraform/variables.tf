variable "subscription_id" {
  type      = string
  sensitive = true
}

variable "environment" {
  type    = string
  default = "dev"
}

variable "aspnetcore_environment" {
  type    = string
  default = "production"
}

variable "location" {
  type    = string
  default = "westeurope"
}

variable "fastnt-base-url" {
  type    = string
}


variable "fastnt-username" {
  type    = string
}


variable "fastnt-password" {
  type    = string
}
