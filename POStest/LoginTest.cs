using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POStest
{
   
    public class Logintest:POSBaseClass
    {

        [Test]
        public void LoginTest1 ()
        {
            getdriver();
            driver.Close();
        }
        


            
            

        }
    }

