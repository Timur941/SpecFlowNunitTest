using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TechTalk.SpecFlow;
using System.Configuration;
using Microsoft.Extensions.Configuration;

namespace SpecFlow_Test.StepDefinitions
{
    [Binding, Scope(Feature = "Отображение списка новостой на странице")]
    public sealed class GpncrNewsStepDefinitions : IDisposable
    {
        // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef


        private readonly ScenarioContext _scenarioContext;
        private readonly IWebDriver driver = new ChromeDriver();
        private readonly IConfigurationRoot config;

        public GpncrNewsStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            config = new ConfigurationBuilder().AddJsonFile("aliases.json").Build();
        }

        public void Dispose()
        {
            driver.Dispose();
        }

        [StepDefinition(@"пользователь открыл страницу ""(.*)""")]
        public void OpenPage(string page)
        {

            var testValue = config["test"];
            Console.WriteLine("asdasda {0}", testValue);
            driver.Navigate().GoToUrl(page);
            Thread.Sleep(2000);
        }

        [StepDefinition(@"""(.*)"" присутствует на странице")]
        public void ElementOnPage(string element)
        {
            driver.FindElement(By.XPath(element));
        }
    }
}
