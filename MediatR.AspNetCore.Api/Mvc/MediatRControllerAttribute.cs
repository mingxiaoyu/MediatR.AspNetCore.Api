using System;

namespace MediatR.AspNetCore.Mvc
{
    public enum HttpMethodEnum
    {
        Delete, Get, Post, Put//, Head, Options, Patch
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class MediatRControllerAttribute : Attribute
    {
        public MediatRControllerAttribute(string controller, string template = "")
        {
            if (string.IsNullOrEmpty(controller))
            {
                throw new ArgumentNullException(nameof(controller));
            }
            Controller = controller;
            Template = template;
        }

        public HttpMethodEnum Method { get; set; }

        public string Controller { get; set; }

        public string Route { get; set; }

        public string Template { get; set; }
    }
}
