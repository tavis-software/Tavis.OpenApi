# Tavis.OpenAPI

This library is a parser for the [OpenAPI Specification](https://openapis.org/).  The model is based around OpenAPI 3.0 specification.

## Simple Example

```csharp
            var parser = new OpenApiParser();
            var openApiDoc = parser.Parse(@"
                    openapi: 3.0.0
                    info:
                        title: A simple inline example
                        version: 1.0.0
                    paths:
                      /api/home:
                        get:
                          responses:
                            200:
                              description: A home document
                    ");

            Assert.Equal("3.0.0", openApiDoc.Version);
```

## Goals

- Import and export OpenAPI specification in both YAML and JSON formats.
- Import downlevel OpenAPI specifications documents for version 2.0 and 1.2
- Enable users to convert old specifications to the latest format.
- Enable applications to transparently support different versions
- Provide comprehensive syntax and semantic error reporting 
- Provide pluggable schema support using specification extensions
- Extend the parsing and validation capabilities to support specification extensions


*** Warning, this project is still very much work in progress ****