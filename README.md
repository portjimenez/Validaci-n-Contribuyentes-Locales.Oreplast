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
- [Configuración inicial](#configuración-inicial)
- [Cómo ejecutar](#cómo-ejecutar)
- [Publicación](#publicación)
- [Seguridad](#seguridad)
- [Notas de mantenimiento](#notas-de-mantenimiento)

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

## Configuración inicial

`Web.config` **no está versionado** porque contiene credenciales. Al clonar el
repositorio:

1. Copiar la plantilla:

   ```powershell
   Copy-Item Web.config.example Web.config
   ```

2. Rellenar los valores reales en la sección `<appSettings>`:

   | Clave | Descripción |
   |---|---|
   | `Entity` | Identificador de la entidad asignado por G4S |
   | `REQUESTOR` | GUID del solicitante autorizado |
   | `Country` | Código de país (`GT`) |
   | `Transaction` | Tipo de transacción (`SYSTEM_REQUEST`) |
   | `User` | GUID del usuario del servicio |
   | `UserName` | Nombre de usuario del servicio |
   | `Data1` | Operación a ejecutar (`CONSULTA_CUI`) |
   | `Data3` | Parámetro adicional (normalmente vacío) |

3. Restaurar los paquetes NuGet:

   ```powershell
   nuget restore identificadores_de_contribuyentes.sln
   ```

   O desde Visual Studio: clic derecho en la solución → **Restaurar paquetes NuGet**.

---

## Cómo ejecutar

1. Abrir `identificadores_de_contribuyentes.sln` en Visual Studio.
2. Compilar la solución (`Ctrl` + `Shift` + `B`).
3. Ejecutar con `F5`. IIS Express levanta el sitio (puerto SSL configurado: `44356`).

La ruta raíz (`/`) apunta directamente a `Contribuyentes.aspx`, definida en
`App_Start/RouteConfig.cs`.

---

## Publicación

El proyecto incluye un perfil de publicación a sistema de archivos en
`Properties/PublishProfiles/FolderProfile.pubxml`, configurado para desplegar en:

```
C:\inetpub\wwwroot\contribuyentes
```

Desde Visual Studio: clic derecho en el proyecto → **Publicar** → seleccionar
`FolderProfile`.

> Después de publicar, verificar que el `Web.config` del servidor tenga las
> credenciales del entorno correspondiente y que `compilation debug` esté en
> `false` en producción.

---

## Seguridad

Este repositorio está configurado para **no exponer credenciales**. Ten en cuenta:

- **`Web.config` está en `.gitignore`.** Contiene los GUID de `REQUESTOR` y `User`
  de los web services. Nunca lo agregues con `git add -f`.
- **`bin/` y `obj/` están ignorados.** El archivo compilado
  `bin/identificadores_de_contribuyentes.dll.config` replica los mismos secretos.
- **`Logs/` está ignorado.** Puede contener CUI, NIT y nombres de personas: son
  datos personales sujetos a protección.
- **Los archivos `*.user` y `*.pubxml.user` están ignorados**, porque pueden incluir
  rutas y credenciales de los servidores de despliegue.
- Si en algún momento se llegara a subir un secreto, **no basta con borrarlo en un
  commit posterior**: hay que limpiar el historial (`git filter-repo` o BFG) y
  **rotar las credenciales con G4S Documenta**.

Consulta `Web.config.example` para saber qué claves debes configurar.

---

## Notas de mantenimiento

- **Archivos huérfanos:** `Datos_contribuyentes.aspx`, `.aspx.cs` y `.aspx.designer.cs`
  son una versión anterior de la página de consulta y **no están incluidos en el
  `.csproj`**, por lo que no se compilan. Se conservan solo como referencia
  histórica; conviene eliminarlos cuando ya no se necesiten.
- **Nombre de la clase:** `Contribuyentes.aspx` hereda de la clase
  `identificadores_de_contribuyentes.Datos_contribuyentes` (herencia del nombre
  original de la página). No es un error, pero puede confundir.
- **Validadores sin uso en la UI:** `CUIValidator` y `NitValidator` implementan la
  validación local del dígito verificador, pero la página actual determina el tipo de
  identificador solo por la longitud del texto. Integrarlos evitaría llamadas
  innecesarias a los web services.
- **Versiones de librerías:** jQuery 3.4.1 y Modernizr 2.8.3 son versiones antiguas;
  vale la pena evaluar su actualización.
