# Identificadores de Contribuyentes

Aplicación web para **validar y consultar identificadores tributarios de Guatemala**: el
**NIT** (Número de Identificación Tributaria) y el **CUI/DPI** (Código Único de
Identificación del Documento Personal de Identificación).

El usuario ingresa un número y el sistema decide automáticamente qué tipo de
identificador es —según la cantidad de dígitos— y consulta el web service
correspondiente para devolver el nombre del contribuyente y sus datos asociados.

---

## Tabla de contenido

- [Stack tecnológico](#stack-tecnológico)
- [Estructura del proyecto](#estructura-del-proyecto)
- [Requisitos previos](#requisitos-previos)
---

## Stack tecnológico

### Backend

| Tecnología | Versión | Uso |
|---|---|---|
| **C#** | LangVersion `default` | Lenguaje de la aplicación |
| **ASP.NET Web Forms** | — | Framework web (páginas `.aspx` con code-behind) |
| **.NET Framework** | 4.7.2 | Plataforma de ejecución |
| **Roslyn** (`Microsoft.CodeDom.Providers.DotNetCompilerPlatform`) | 2.0.1 | Compilador de C#/VB en runtime |
| **WCF** (`System.ServiceModel`) | — | Cliente SOAP para los web services externos |
| **Newtonsoft.Json** | 13.0.3 | Deserialización de las respuestas JSON del servicio de DPI |

Es un proyecto **ASP.NET Web Forms clásico**, no ASP.NET Core. Se compila como
biblioteca (`OutputType: Library`) y se hospeda en **IIS** / **IIS Express**.

### Frontend

| Tecnología | Versión | Uso |
|---|---|---|
| **Bootstrap** | 5.2.3 | Sistema de grillas y componentes de UI |
| **jQuery** | 3.4.1 | Manipulación del DOM |
| **Modernizr** | 2.8.3 | Detección de características del navegador |
| **Microsoft Ajax** (`ScriptManager`) | 5.0.0 | Postbacks parciales (`UpdatePanel`) |

### Infraestructura del proyecto

| Componente | Uso |
|---|---|
| **NuGet** con `packages.config` | Gestión de dependencias (formato clásico, carpeta `packages/`) |
| **Microsoft.AspNet.Web.Optimization** + **WebGrease** | Bundling y minificación de CSS/JS |
| **Microsoft.AspNet.FriendlyUrls** 1.0.2 | URLs limpias sin la extensión `.aspx` |
| **Visual Studio** (`.sln` + `.csproj` legacy) | IDE y sistema de compilación (MSBuild) |

---

### Lógica de enrutamiento de la consulta

En `Contribuyentes.aspx.cs`, el número ingresado se limpia de guiones y espacios y
luego se evalúa por longitud:

- **13 dígitos** → se trata como **CUI/DPI** y se usa la respuesta de `ConsultaDPI`
  (devuelve `CUI`, `Nombre` y `Fallecido`, deserializados con Newtonsoft.Json).
- **Cualquier otra longitud** → se trata como **NIT** y se usa la respuesta de
  `ConsultaNIT` (devuelve `NIT` y `nombre`).

---

## Estructura del proyecto

```
identificadores_de_contribuyentes/
├── App_Start/
│   ├── BundleConfig.cs           # Bundles de JS (WebForms, MsAjax, Modernizr)
│   └── RouteConfig.cs            # FriendlyUrls; ruta raíz "" → Contribuyentes.aspx
├── Connected Services/
│   ├── ConsultaDPI/              # WSDL + cliente SOAP generado (FactWSFront)
│   └── ConsultaNIT/              # WSDL + cliente SOAP generado
├── Content/                      # CSS (Bootstrap 5 + Site.css)
├── Scripts/                      # JS (jQuery, Bootstrap, Modernizr, WebForms/MSAjax)
├── Properties/
│   ├── AssemblyInfo.cs
│   └── PublishProfiles/          # Perfil de publicación a carpeta
├── Contribuyentes.aspx(.cs)      # Página principal de consulta  <- página activa
├── CUIValidator.cs               # Validación del CUI/DPI guatemalteco
├── NitValidator.cs               # Validación del NIT guatemalteco
├── Site.Master                   # Layout principal
├── Site.Mobile.Master            # Layout para dispositivos móviles
├── ViewSwitcher.ascx             # Alternar entre vista móvil y escritorio
├── Global.asax(.cs)              # Arranque de la aplicación
├── Web.config                    # Configuración (NO se versiona - ver Seguridad)
├── Web.config.example            # Plantilla de configuración
└── packages.config               # Dependencias NuGet
```

### Validadores de dominio

Son la lógica propia del proyecto, independiente de los web services:

- **`CUIValidator.cs`** — valida el CUI con expresión regular, verifica el dígito
  verificador mediante el algoritmo módulo 11 y comprueba que el código de
  departamento y municipio sean válidos según el listado oficial de Guatemala.
- **`NitValidator.cs`** — valida el formato del NIT con expresión regular y verifica
  su dígito verificador (que puede ser `k`/`K`).

---

## Requisitos previos

- **Windows** con **IIS** o **IIS Express**
- **Visual Studio 2019 / 2022** con la carga de trabajo *Desarrollo de ASP.NET y web*
- **.NET Framework 4.7.2 Developer Pack**
- Conectividad HTTPS de salida hacia `fel.g4sdocumenta.com`
- Credenciales válidas de G4S Documenta (Entity, Requestor, User)

---
