# Tavis.OpenApi
## 0.8.7
- Write support for optional schema documentation fields
## 0.8.6
- More writing support for extensions
## 0.8.5
- Support for writing extensions
## 0.8.4
- Bug fixes (sorry!)
## 0.8.2
- Added back StrongNaming cert.
- Added back support for net46
## 0.8.1
- Fixes to JSON Writer
- Added support for rendering V2 host,basePath, schemes, produces, consumes, requestBody, parameters
- Improved error handling
## 0.8.0
- Moved to .net Standard 2.0
- Some bug fixes for parsing badly structured input
- Enabled referencing several model objects that previously were not supported
## 0.7.1 
- Added YAML/JSON output selector to Workbench tool
- Lots of JSON serialization fixes
- Fix to YAML serialization of multi-line text
## 0.7.0
- Added parameter to constructor of OpenAPIV3Writer to accept different writers
- Added Save method to OpenApiDocument to simplify serialziation
- Made OpenApiParser methods static and return ParsingContext instead of OpenApiDocument
- Overall structural reorganization

## 0.6.0
- Fixed name of OpenAPIV3Writer.Write
- Signed binary with self-signed key to support Strong named scenarios
- Added support for parsing Json Schema strings directly into OpenAPI Schema objects

## 0.5.1
- Working on support for $ref

## 0.5.0
- First version. 
