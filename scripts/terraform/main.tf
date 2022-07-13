terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = ">= 3.13.0"
    }
  }
}

provider "azurerm" {
  features {}
}

resource "azurerm_resource_group" "rg" {
  name     = "GeneratingChaosResourceGroup"
  location = "<desidered region>"

   tags = {
    environment = "Demo"
  }
}

resource "azurerm_container_registry" "container" {
  name                = "<Your Container Registry Name>"
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  sku                 = "Basic"
  admin_enabled       = true
}

resource "azurerm_kubernetes_cluster" "cluster" {
  name                = "<Your k8s Cluster Name>"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  dns_prefix          = "<Your k8s Cluster Name>"
  

  default_node_pool {
    name       = "default"
    node_count = "3"
    vm_size    = "standard_d2_v2"
    os_disk_size_gb = 30
  }

  identity {
    type = "SystemAssigned"
  }

  tags = {
    environment = "Demo"
  }
}

resource "azurerm_role_assignment" "role" {
  principal_id                     = azurerm_kubernetes_cluster.cluster.kubelet_identity[0].object_id 
  role_definition_name             = "AcrPull"
  scope                            = azurerm_container_registry.container.id
  skip_service_principal_aad_check = true
}