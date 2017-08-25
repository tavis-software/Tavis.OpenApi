# Tavis.OpenAPI

This library is a parser for the [OpenAPI Specification](https://openapis.org/).  The model is based around OpenAPI 3.0 specification.

## Simple Example

```csharp
            var parsingContext = OpenApiParser.Parse(@"
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

            Assert.Equal("3.0.0", parsingContext.OpenApiDoc.Version);
            Assert.Equal(0, parsingContext.ParsingErrors.Count());
```

## Goals

- Import OpenAPI V3 definitions in both YAML and JSON formats.
- Export OpenAPI definition in YAML format
- Import OpenAPI V2 definitions
- Provide comprehensive syntax and semantic error reporting 
- Enable constructing of OpenAPI descriptions via a document object model
