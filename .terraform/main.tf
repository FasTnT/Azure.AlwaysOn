terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = ">= 2.26"
    }
  }
  backend "azurerm" {
    storage_account_name = ""
    access_key           = ""
    container_name       = "tfstate"
    key                  = "terraform.tfstate"
  }

  required_version = ">= 1.0.0"
}

provider "azurerm" {
  features {}

  subscription_id = var.subscription_id
}

locals {
  tags = {
    Environment = upper(var.environment)
  }
  base_resource_name = lower("fastnt-alwayson-${var.environment}")
}

resource "azurerm_resource_group" "fastnt_main" {
  name     = local.base_resource_name
  location = var.location
  tags     = local.tags
}

resource "azurerm_storage_account" "fastnt_storage" {
  name                     = "fastntalwayson${var.environment}st"
  resource_group_name      = azurerm_resource_group.fastnt_main.name
  location                 = var.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
}

resource "azurerm_app_service_plan" "fastnt_always_on_plan" {
  name                = "azure-functions-test-service-plan"
  location            = var.location
  resource_group_name = azurerm_resource_group.fastnt_main.name
  kind                = "FunctionApp"

  sku {
    tier = "Dynamic"
    size = "Y1"
  }
}

resource "azurerm_application_insights" "fastnt_always_on_appi" {
  name                = "${local.base_resource_name}-alwayson-func-appi"
  location            = var.location
  resource_group_name = azurerm_resource_group.fastnt_main.name
  application_type    = "web"
  tags                = local.tags
}

resource "azurerm_function_app" "fastnt_always_on" {
  name                       = "${local.base_resource_name}-alwayson-func"
  location                   = var.location
  resource_group_name        = azurerm_resource_group.fastnt_main.name
  app_service_plan_id        = azurerm_app_service_plan.fastnt_always_on_plan.id
  storage_account_name       = azurerm_storage_account.fastnt_storage.name
  storage_account_access_key = azurerm_storage_account.fastnt_storage.primary_access_key
  version                    = "~2"
  tags                       = local.tags  
  app_settings = {
    "APPINSIGHTS_INSTRUMENTATIONKEY" = "${azurerm_application_insights.fastnt_always_on_appi.instrumentation_key}"
    "APPLICATIONINSIGHTS_CONNECTION_STRING" : "${azurerm_application_insights.fastnt_always_on_appi.connection_string}"
    "ApplicationInsightsAgent_EXTENSION_VERSION" : "~2"
    "PingEpcisRepository.Trigger.Cron" : "0 */4 * * * *"
    "EpcisRepository.Url" : "${var.fastnt-base-url}"
    "EpcisRepository.Authorization.Username": "${var.fastnt-username}"
    "EpcisRepository.Authorization.Password": "${var.fastnt-password}"
  }
  
  source_control {
	repo_url = "https://github.com/FasTnT/Azure.AlwaysOn"
	branch   = "master"
  }
}