using OpenQA.Selenium;
using POStest.Selenium;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POStest.TestFramework
{

    public static class DriverJQueryHelper
    {
        public static void JQueryAction(By by, string jqueryFunctionSting)
        {
            string script = GetJQueryWrappedString(by) + "." + jqueryFunctionSting;
            Driver.ExecuteScript(script);
        }

        //exetutes the function in the browser and returns the result
        public static string JQueryFunction(string jqueryFunctionSting)
        {
            string a = "test";
            try
            {
                Driver.ExecuteScript(@"$('body').append('<em id=""asdfas5d43asd54sa""></em>');");
                Driver.ExecuteScript("$('#asdfas5d43asd54sa').attr('value',function(){return " + jqueryFunctionSting + "}).attr('id','asdfas5d43asd54saDONE');");
            //    string returnValue = Driver.Find(By.Id("asdfas5d43asd54saDONE")).GetAttribute("value");
                Driver.ExecuteScript("$('#asdfas5d43asd54saDONE').remove();");

                return a;
            }
            catch
            {
                return null;
            }
        }

        public static string GetJQueryWrappedString(this By by)
        {
            string byString = by.ToString();
            string jqueryReturnString = string.Empty;

            if (byString.StartsWith("By.Name: ")) { jqueryReturnString = CombineJqueryString(byString, "By.Name: ", "$('[name=", "')"); }
            if (byString.StartsWith("By.XPath: ")) { jqueryReturnString = $"document.evaluate({CombineJqueryString(byString, "By.XPath: ", "'", "'")}, document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.textContent"; }
            if (byString.StartsWith("By.CssSelector: ")) { jqueryReturnString = CombineJqueryString(byString, "By.CssSelector: ", "$('", "')"); }
            if (byString.StartsWith("By.Id: ")) { jqueryReturnString = CombineJqueryString(byString, "By.Id: ", "$('#", "')"); }
            if (byString.StartsWith("By.LinkText: ")) { jqueryReturnString = CombineJqueryString(byString, "By.LinkText: ", @"$('a:contains(""", @""")')"); }
            if (byString.StartsWith("By.PartialLinkText: ")) { jqueryReturnString = CombineJqueryString(byString, "By.PartialLinkText: ", @"$('a:contains(""", @""")')"); }
            if (byString.StartsWith("By.ClassName[Contains]: ")) { jqueryReturnString = CombineJqueryString(byString, "By.ClassName[Contains]: ", "$('.", "')"); }
            if (byString.StartsWith("By.TagName: ")) { jqueryReturnString = CombineJqueryString(byString, "By.TagName: ", "$('", "')"); }

            return jqueryReturnString;
        }

        private static string CombineJqueryString(string byString, string replace, string before, string after)
        {
            string value = byString.Replace(replace, "");
            string script = before + value + after;

            return script;
        }

        public static bool IsElementPresent(By by)
        {
            string functionSting = by.GetJQueryWrappedString() + ".length > 0";
            return bool.Parse(JQueryFunction(functionSting));
        }

        public static bool IsParentDisabled(By by)
        {
            return bool.Parse(JQueryFunction(by.GetJQueryWrappedString() + ".closest('[disabled]').length > 0"));
        }

        public static bool IsParentSelected(By by, int index = 0)
        {
            return bool.Parse(JQueryFunction(by.GetJQueryWrappedString() + string.Format(".eq({0}).closest('[class*=selected]').length > 0", index)));
        }

        public static bool IsDisabled(By by)
        {
            return bool.Parse(JQueryFunction(by.GetJQueryWrappedString() + ".closest('[disabled]').length > 0"));
        }

        public static bool JQueryIsAngularInvalid(By by)
        {
            return bool.Parse(JQueryFunction(by.GetJQueryWrappedString() + ".hasClass('ng-invalid')"));
        }

        public static bool IsSiblingContainsAttribute(this By by, string attributeName, string attributeValue)
        {
            string value = JQueryFunction(by.GetJQueryWrappedString() + $".siblings().attr('{attributeName}')");

            return string.IsNullOrEmpty(value) ? false : value.Equals(attributeValue);
        }

        public static int GetWrappedSetLength(By by)
        {
            return int.Parse(JQueryFunction(by.GetJQueryWrappedString() + ".length"), CultureInfo.InvariantCulture);
        }

        public static bool GetWrappedHasClass(By by, string className)
        {
            return JQueryFunction(by.GetJQueryWrappedString() + ".hasClass('" + className + "')") == "true";
        }

        public static bool GetWrappedHasClass(string jqueryFunctionSting, string className)
        {
            return JQueryFunction(jqueryFunctionSting + ".hasClass('" + className + "')") == "true";
        }

        public static string GetWrappedSetText(By by)
        {
            return JQueryFunction(by.GetJQueryWrappedString() + ".text()");
        }

        public static void PutValuesIntoAllInputFields(By by, string sendKeys)
        {
            int numberOfInputFields = GetWrappedSetLength(by);

            for (int i = 0; i < numberOfInputFields; i++)
            {
                FillInputFields(by, sendKeys, i);
            }
        }

        public static void PutValuesIntoAllInputFields(By by, Func<string> sendKeysMethod)
        {
            int numberOfInputFields = GetWrappedSetLength(by);

            for (int i = 0; i < numberOfInputFields; i++)
            {
                FillInputFields(by, new Func<string>(sendKeysMethod)(), i);
            }
        }

        private static void FillInputFields(By by, string sendKeys, int index)
        {
            Driver.Find(by, index).SendKeys(sendKeys);
        }

        public static int GetInnerWidth()
        {
            int innerWidth = int.Parse(JQueryFunction("window.innerWidth"));

            return innerWidth;
        }

        public static int GetInnerHeight()
        {
            int innerHeight = int.Parse(JQueryFunction("window.innerHeight"));

            return innerHeight;
        }

        //public static void SetInputFields(VirtualKeyboardInputType inputType = VirtualKeyboardInputType.text)
        //{
        //    Driver.ExecuteScript($@"$('body').scope().clientParameterMap.deviceInputType = '{inputType.ToString()}';");
        //}

        public static void SetVirtualKeyboard(bool turnOn)
        {
            Driver.ExecuteScript($"_ommitConfiguration_ = {(!turnOn).ToString().ToLowerInvariant()}");
            ProjSettings.IsVirtualKeyboardEnabled = turnOn;
        }
    }
}

