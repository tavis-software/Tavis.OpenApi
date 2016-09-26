//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;

//namespace Tavis.OpenApi.Model
//{
//    public static class OpenApiVocab
//    {

//        public static VocabTerm<OpenApiDocument> Create()
//        {
//            var openApiTerm = new VocabTerm<OpenApiDocument>();

//            openApiTerm.MapProperty<string>("swagger", (s, o) => s.Version = o);

//            openApiTerm.MapProperty<string>("schemes", (s, o) =>
//            {
//                if (s.Schemes == null) s.Schemes = new List<String>();
//                s.Schemes.Add(o);
//            });

//            var infoTerm = new VocabTerm<Info>("info");
//            infoTerm.MapProperty<string>("description",(s,o) => s.Description = o);
//            infoTerm.MapProperty<string>("termsOfService", (s, o) => s.TermsOfService = o);
//            infoTerm.MapProperty<string>("title", (s, o) => s.Title = o);

//            openApiTerm.MapObject<Info>(infoTerm, (s) =>
//            {
//                s.Info = new Info();
//                return s.Info;
//            });

//            var contactTerm = new VocabTerm<Contact>("contact");
//            infoTerm.MapObject<Contact>(contactTerm, (s) =>
//            {
//                s.Contact = new Contact();
//                return s.Contact;
//            });

//            var opsTerm = new VocabTerm<Operation>();
//            opsTerm.MapProperty<string>("operationId", (s, o) => s.Id = o);
//            //opsTerm.MapProperty<string>("x-controller", (s, o) => s. = o);

//            var pathTerm = new VocabTerm<Path>();
//            pathTerm.MapAnyObject<Operation>(opsTerm, (s, p) => {
//                return s.AddOperation(p, Guid.NewGuid().ToString());
//            });

//            var pathsTerm = new VocabTerm<OpenApiDocument>("paths");

//            pathsTerm.MapAnyObject<Path>(pathTerm, (s, p) => {
//                return s.AddPath(p);
//            });

//            openApiTerm.MapObject<OpenApiDocument>(pathsTerm, (s) =>
//            {
//                return s;
//            });


//            return openApiTerm;
//        }


//    }
//}