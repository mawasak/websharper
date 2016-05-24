﻿using Microsoft.FSharp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSharper;
using WebSharper.Sitelets;
using Elt = WebSharper.Sitelets.Tests.Server.Elt;
using Attr = WebSharper.Sitelets.Tests.Server.Attr;
using Text = WebSharper.Sitelets.Tests.Server.Text;

namespace WebSharper.CSharp.Sitelets.Tests
{
    public class SiteletTest
    {
        public static Sitelet<object> Main =>
            new SiteletBuilder()
                .With("/", ctx =>
                    Content.Page<object>(
                        Body:
                            Elt("div",
                                Elt("div",
                                    Elt("a", Attr("href", ctx.Link.Invoke(JohnDoe)),
                                        Text("Go to John Doe's page"))),
                                Elt("form",
                                    Attr("action", ctx.Link.Invoke(EmptyQueryPerson)),
                                    Elt("input", Attr("name", "first"), Attr("value", "Jane")),
                                    Elt("input", Attr("name", "last"), Attr("value", "Smith")),
                                    Elt("input", Attr("name", "age"), Attr("type", "number"), Attr("value", "42")),
                                    Elt("input", Attr("type", "submit"))))))
                .With<Person>((ctx, person) =>
                    Content.Page<Person>(
                        Body:
                            Elt("div",
                                Text(String.Format("{0} {1} is {2} years old. ",
                                    person.name.first, person.name.last, person.age)),
                                Elt("a", Attr("href", ctx.Link.Invoke("/")),
                                    Text("Go back to C# sitelets tests home")))))
                .With<QueryPerson>((ctx, person) =>
                    Content.Page<QueryPerson>(
                        Body:
                            Elt("div",
                                person.age.HasValue ?
                                    Text(String.Format("{0} {1} is {2} years old. ",
                                        person.name.first, person.name.last, person.age.Value)) :
                                    Text(String.Format("{0} {1} won't tell their age.",
                                        person.name.first, person.name.last)),
                                Elt("a", Attr("href", ctx.Link.Invoke("/")),
                                    Text("Go back to C# sitelets tests home")))))
                .Install();

        [EndPoint("/person")]
        private class Person
        {
            public Name name;
            public int age;
        }

        private class Name
        {
            public string first;
            public string last;
        }

        [EndPoint("/person")]
        private class QueryPerson
        {
            public QueryName name;

            [Query]
            public int? age;
        }

        private class QueryName
        {
            [Query]
            public string first;

            [Query]
            public string last;
        }

        private static Elt Elt(string name, params Web.INode[] children) => new Elt(name, children);
        private static Attr Attr(string name, string value) => new Attr(name, value);
        private static Text Text(string text) => new Text(text);
        private static Person JohnDoe => new Person { name = new Name { first = "John", last = "Doe" }, age = 30 };
        private static QueryPerson EmptyQueryPerson => new QueryPerson { name = new QueryName() };
    }
}