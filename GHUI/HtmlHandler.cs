using System;
using System.Globalization;
using System.Reflection;
using mshtml;

namespace GHUI
{
    class HtmlHandler : IReflect
    {
        public HtmlHandler(EventHandler evHandler, IHTMLWindow2 sourceWindow)
        {
            eventHandler = evHandler;
            htmlWindow = sourceWindow;
        }

        public IHTMLWindow2 SourceHTMLWindow => htmlWindow;

        #region IReflect

        FieldInfo IReflect.GetField(string name, BindingFlags bindingAttr)
        {
            return typeIReflectImplementation.GetField(name, bindingAttr);
        }

        FieldInfo[] IReflect.GetFields(BindingFlags bindingAttr)
        {
            return typeIReflectImplementation.GetFields(bindingAttr);
        }

        MemberInfo[] IReflect.GetMember(string name, BindingFlags bindingAttr)
        {
            return typeIReflectImplementation.GetMember(name, bindingAttr);
        }

        MemberInfo[] IReflect.GetMembers(BindingFlags bindingAttr)
        {
            return typeIReflectImplementation.GetMembers(bindingAttr);
        }

        MethodInfo IReflect.GetMethod(string name, BindingFlags bindingAttr)
        {
            return typeIReflectImplementation.GetMethod(name, bindingAttr);
        }

        MethodInfo IReflect.GetMethod(string name, BindingFlags bindingAttr, Binder binder, Type[] types,
            ParameterModifier[] modifiers)
        {
            return typeIReflectImplementation.GetMethod(name, bindingAttr, binder, types, modifiers);
        }

        MethodInfo[] IReflect.GetMethods(BindingFlags bindingAttr)
        {
            return typeIReflectImplementation.GetMethods(bindingAttr);
        }

        PropertyInfo[] IReflect.GetProperties(BindingFlags bindingAttr)
        {
            return typeIReflectImplementation.GetProperties(bindingAttr);
        }

        PropertyInfo IReflect.GetProperty(string name, BindingFlags bindingAttr)
        {
            return typeIReflectImplementation.GetProperty(name, bindingAttr);
        }

        PropertyInfo IReflect.GetProperty(string name, BindingFlags bindingAttr, Binder binder, Type returnType,
            Type[] types, ParameterModifier[] modifiers)
        {
            return typeIReflectImplementation.GetProperty(name, bindingAttr, binder, returnType, types, modifiers);
        }

        object IReflect.InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args,
            ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
        {
            if (name == "[DISPID=0]")
            {
                if (eventHandler != null)
                {
                    eventHandler(this, EventArgs.Empty);
                }
            }

            return null;
        }

        Type IReflect.UnderlyingSystemType => typeIReflectImplementation.UnderlyingSystemType;

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