using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using OpenQA.Selenium.Support.UI;
using System.Threading;


namespace NathanEgbertFinal
{
    public class Tests
    {
        ChromeDriver driver;
        private string email = "automationfinalne@automation.com";
        private string password = "AutomationFinalNE";

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();


        }

        [Test, Category("Smoke Test"), Order(1)]
        public void Login()
        {
            driver.Navigate().GoToUrl("https://candidatex:qa-is-cool@qa-task.backbasecloud.com/");
            driver.Manage().Window.Maximize();

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);


            IWebElement SignInButton = driver.FindElement(By.LinkText("Sign in"));
            SignInButton.Click();

            IWebElement Email = driver.FindElement(By.CssSelector("input[formcontrolname='email']"));
            IWebElement Password = driver.FindElement(By.CssSelector("input[formcontrolname='password']"));
            IWebElement SubmitButton = driver.FindElement(By.CssSelector("button[type='submit']"));

            Email.SendKeys(email);
            Password.SendKeys(password);
            SubmitButton.Click();


            //TODO: Fix to be resuable 
            IWebElement profileHome = driver.FindElement(By.CssSelector("a[href='/profile/automationfinalne']"));

            Assert.AreEqual(profileHome.Text, "automationfinalne");
        }

        [Test, Category("Smoke Test"), Order(2)]
        public void CreateArticle()
        {
            Login();

            string bodyComment = "This is my automation final article post";

            IWebElement NewArticle = driver.FindElement(By.CssSelector("a[href='/editor']"));
            NewArticle.Click();

            IWebElement ArticleTitle = driver.FindElement(By.CssSelector("input[formcontrolname='title']"));
            ArticleTitle.SendKeys("Automation final comment");

            IWebElement Description = driver.FindElement(By.CssSelector("input[formcontrolname='description']"));
            Description.SendKeys("This aricle is about automation");

            //TODO:resuable code, refactor later
            IWebElement Body = driver.FindElement(By.CssSelector("textarea[formcontrolname='body']"));
            Body.SendKeys(bodyComment);

            IWebElement Tags = driver.FindElement(By.CssSelector("input[placeholder='Enter tags']"));
            Tags.SendKeys("Tag one");

            IWebElement PublishButton = driver.FindElement(By.CssSelector("button[type='button']"));
            PublishButton.Click();

            Assert.AreEqual(bodyComment, driver.FindElement(By.XPath("/html/body/app-root/app-article-page/div/div[2]/div[1]/div/div/p")).Text);

        }

        [Test, Category("Smoke Test"), Order(3)]
        public void UpdateArticle()
        {
            Login();

            //TODO: Refactor to make resuable 
            IWebElement profileHome = driver.FindElement(By.CssSelector("a[href='/profile/automationfinalne']"));
            profileHome.Click();

            //TODO: reusable code, refactor later
            var profileArticles = driver.FindElements(By.CssSelector("div[class='article-preview']"));
                profileArticles[0].Click();

            IWebElement EditArticle = driver.FindElement(By.XPath("/html/body/app-root/app-article-page/div/div[1]/div/app-article-meta/div/span[1]/a"));
            EditArticle.Click();

            //TODO: resuable code in CreateArticle method, refactor later
            driver.FindElement(By.CssSelector("textarea[formcontrolname='body']")).Clear();
            driver.FindElement(By.CssSelector("textarea[formcontrolname='body']")).SendKeys("Updated");
            driver.FindElement(By.CssSelector("button[type='button']")).Click();


            Assert.AreEqual("Updated", driver.FindElement(By.XPath("/html/body/app-root/app-article-page/div/div[2]/div[1]/div/div/p")).Text);

        }

        [Test, Category("Smoke Test"), Order(4)]
        public void DeleteArticle()
        {

            Login();

            //TODO: Refactor to make resuable 
            IWebElement profileHome = driver.FindElement(By.CssSelector("a[href='/profile/automationfinalne']"));
            profileHome.Click();

            //TODO: reusable code, refactor later
            var profileArticles = driver.FindElements(By.CssSelector("div[class='article-preview']"));
            profileArticles[0].Click();

            driver.FindElement(By.CssSelector("button[class='btn btn-sm btn-outline-danger']")).Click();
            profileArticles = driver.FindElements(By.CssSelector("div[class='article-preview']"));

            //IWebElement NoArticlesText = driver.FindElement(By.XPath("/html/body/app-root/app-home-page/div/div/div/div[1]/app-article-list/div[2]"));

            Assert.IsEmpty(profileArticles);
        }

        [Test, Category("Smoke Test")]
        public void CreateReadDeleteComments()
        {
            Login();

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);

            IWebElement GlobalFeed = driver.FindElement(By.LinkText("Global Feed"));
            GlobalFeed.Click();

            //reusable code, refactor later
            var feedArticles = driver.FindElements(By.CssSelector("div[class='article-preview']"));
            feedArticles[0].Click();

            IWebElement textBox = driver.FindElement(By.XPath("/html/body/app-root/app-article-page/div/div[2]/div[3]/div/div/form/fieldset/div[1]/textarea"));

            string comment = "This is a comment";

            textBox.SendKeys(comment);

            driver.FindElement(By.CssSelector("button[type='submit']")).Click();


            //reading the comment that was left
            IWebElement commentLeft = driver.FindElement(By.CssSelector("p[class='card-text']"));
            Assert.AreEqual(comment, commentLeft.Text);

            //delete comment
            driver.FindElement(By.XPath("/html/body/app-root/app-article-page/div/div[2]/div[3]/div/app-article-comment/div/div[2]/span[2]/i")).Click();

        }

        [Test, Category("Smoke Test")]
        public void FollowUser()
        {
            Login();

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);

            IWebElement GlobalFeed = driver.FindElement(By.LinkText("Global Feed"));
            GlobalFeed.Click();

            //TODO: Fix this with a better wait
            Thread.Sleep(TimeSpan.FromSeconds(2));

            //TODO: reusable code, refactor later
            var feedArticles = driver.FindElements(By.CssSelector("div[class='article-preview']"));
            feedArticles[0].Click();

            driver.FindElement(By.XPath("/html/body/app-root/app-article-page/div/div[1]/div/app-article-meta/div/span[2]/app-follow-button/button")).Click();

            Thread.Sleep(TimeSpan.FromSeconds(2));

            string unfollowButton = driver.FindElement(By.XPath("/html/body/app-root/app-article-page/div/div[1]/div/app-article-meta/div/span[2]/app-follow-button/button")).Text;

            //assert unfollow text to verify use is followed
            Assert.IsTrue(unfollowButton.Contains("Unfollow"));


        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}