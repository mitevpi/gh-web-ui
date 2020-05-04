using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using mshtml;

namespace GHUI
{
    class HtmlHandler : IReflect
    {
        public HtmlHandler(EventHandler evHandler, IHTMLWindow2 sourceWindow)
        {
            this.eventHandler = evHandler;
            this.htmlWindow = sourceWindow;
        }

        public IHTMLWindow2 SourceHTMLWindow
        {
            get { return this.htmlWindow; }
        }

        #region IReflect

        FieldInfo IReflect.GetField(string name, BindingFlags bindingAttr)
        {
            return this.typeIReflectImplementation.GetField(name, bindingAttr);
        }

        FieldInfo[] IReflect.GetFields(BindingFlags bindingAttr)
        {
            return this.typeIReflectImplementation.GetFields(bindingAttr);
        }

        MemberInfo[] IReflect.GetMember(string name, BindingFlags bindingAttr)
        {
            return this.typeIReflectImplementation.GetMember(name, bindingAttr);
        }

        MemberInfo[] IReflect.GetMembers(BindingFlags bindingAttr)
        {
            return this.typeIReflectImplementation.GetMembers(bindingAttr);
        }

        MethodInfo IReflect.GetMethod(string name, BindingFlags bindingAttr)
        {
            return this.typeIReflectImplementation.GetMethod(name, bindingAttr);
        }

        MethodInfo IReflect.GetMethod(string name, BindingFlags bindingAttr, Binder binder, Type[] types,
            ParameterModifier[] modifiers)
        {
            return this.typeIReflectImplementation.GetMethod(name, bindingAttr, binder, types, modifiers);
        }

        MethodInfo[] IReflect.GetMethods(BindingFlags bindingAttr)
        {
            return this.typeIReflectImplementation.GetMethods(bindingAttr);
        }

        PropertyInfo[] IReflect.GetProperties(BindingFlags bindingAttr)
        {
            return this.typeIReflectImplementation.GetProperties(bindingAttr);
        }

        PropertyInfo IReflect.GetProperty(string name, BindingFlags bindingAttr)
        {
            return this.typeIReflectImplementation.GetProperty(name, bindingAttr);
        }

        PropertyInfo IReflect.GetProperty(string name, BindingFlags bindingAttr, Binder binder, Type returnType,
            Type[] types, ParameterModifier[] modifiers)
        {
            return this.typeIReflectImplementation.GetProperty(name, bindingAttr, binder, returnType, types, modifiers);
        }

        object IReflect.InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args,
            ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
        {
            if (name == "[DISPID=0]")
            {
                if (this.eventHandler != null)
                {
                    this.eventHandler(this, EventArgs.Empty);
                }
            }

            return null;
        }

        Type IReflect.UnderlyingSystemType
        {
            get { return this.typeIReflectImplementation.UnderlyingSystemType; }
        }

        #endregion


        private IReflect typeIReflectImplementation = typeof(HtmlHandler);
        private EventHandler eventHandler;
        private IHTMLWindow2 htmlWindow;

        private static void attachOnChangeEvent(HTMLDocument doc, bool flag)
        {
            // Install onchange for <input type=password, text, file>.

            IHTMLElementCollection inputElementList = doc.getElementsByTagName("input");
            foreach (IHTMLElement el in inputElementList)
            {
                string inputType = el.getAttribute("type") as string;
                if (("text" == inputType) || ("password" == inputType) || ("file" == inputType))
                {
                    IHTMLElement2 inputElement = el as IHTMLElement2;
                    IHTMLWindow2 wnd = (el.document as IHTMLDocument2).parentWindow;
                    inputElement.attachEvent("onchange", new HtmlHandler(onchangeInput, wnd));
                }
            }
        }


        private static void onchangeInput(object sender, EventArgs e)
        {
            HtmlHandler htmlHandler = (HtmlHandler) sender;
            IHTMLElement element = htmlHandler.SourceHTMLWindow.@event.srcElement;

            //Do whatever i want with the element
        }
    }
}