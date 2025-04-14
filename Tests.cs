using System.Security.Cryptography.X509Certificates;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V133.FedCm;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace _1_test;

public class Tests
{   
    private IWebDriver _driver;

    private WebDriverWait _wait;
    [SetUp]
    public void Setup()
    {
        var options = new ChromeOptions();
        options.AddArguments("--no-sandbox","--start-maximized","--disable-extensions");
        
        _driver = new ChromeDriver(options);
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
         _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));

        Login("misterpeka@gmail.com","Aaaaa12345!");
    }

    private void Login(string username, string password)
    {
        _driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru");  
        _driver.FindElement(By.Id("Username")).SendKeys(username);
        _driver.FindElement(By.Id("Password")).SendKeys(password);
        _driver.FindElement(By.Name("button")).Click();

        _wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("[data-tid='Title']")));
    }
    [TearDown]
    public void TearDown()
    {
        _driver.Quit();
        _driver.Dispose();
    }

    [Test]
    public void Logout_test()
    {
         var profile_button = _driver.FindElement(By.CssSelector("[data-tid='DropdownButton']"));
         profile_button.Click();

         var Logout_button = _driver.FindElement(By.CssSelector("[data-tid='Logout']"));
         Logout_button.Click();
         
         Assert.That(_driver.Url.Contains("https://staff-testing.testkontur.ru/Account/Logout"), "Не тот URL");
    }

 [Test]
    public void Adding_new_comment()
    {
        _driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/comments");

        var comment = _driver.FindElement(By.CssSelector("[data-tid='AddComment']"));
        comment.Click();

        var comment_field = _driver.FindElement(By.CssSelector("[data-tid='CommentInput']"));
        comment_field.SendKeys("Test comment");

        var button = _driver.FindElement(By.CssSelector("[data-tid='SendComment']"));
        button.Click();
        _wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("[data-tid='RemoveComment']")));

        var comment_text= _driver.FindElement(By.CssSelector("[data-tid='TextComment']"));

        Assert.That(comment_text.Text, Does.Contain("Test comment"), "Комментарий не появился");

   
    }

     [Test]
      public void Change_second_email()
      {
        _driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/profile/settings/edit");
  
        var second_email = _driver.FindElement(By.CssSelector("[data-tid='AdditionalEmail'] [data-tid='Input']"));
        second_email.SendKeys(Keys.Control + "a");
        second_email.SendKeys(Keys.Delete);
        second_email.SendKeys("SecondEmail@mail.ru");

        var save = _driver.FindElement(By.CssSelector("[class='sc-juXuNZ kVHSha'] [fill-rule='evenodd']"));
        save.Click();

        _wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("[data-tid='CalendarToday']")));

        var Contact = _driver.FindElement(By.CssSelector("[data-tid='ContactCard']"));
        Assert.That(Contact.Text, Does.Contain("SecondEmail@mail.ru"), "Почта не изменилась");
      }

      [Test]
      public void Create_сommunity()
      {
        _driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/communities");

        var create_button = _driver.FindElement(By.CssSelector("button.vPeNx"));
        create_button.Click();
        _wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("[data-tid='CancelButton']")));

        var community_name = _driver.FindElement(By.CssSelector("[data-tid='Name']"));
        community_name.SendKeys("New community");

        var create_community = _driver.FindElement(By.CssSelector("[data-tid='CreateButton']"));
        create_community.Click();
        _wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("[data-tid='UploadFiles']")));

        var title = _driver.FindElement(By.CssSelector("[data-tid='Title']"));
        Assert.That(title.Text, Does.Contain("New community"), "Не удалось создать сообщество");

      }

      [Test]

      public void Create_folder()
      {
        _driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/files");
   
        var drop_button = _driver.FindElement(By.CssSelector("[data-tid='DropdownButton'] [class='react-ui-1f3jmd3']"));
        drop_button.Click();

        var create_in_drop = _driver.FindElement(By.CssSelector("[data-tid='CreateFolder']"));
        create_in_drop.Click();

        var folder_name = _driver.FindElement(By.CssSelector("[data-tid='Input']"));
        folder_name.SendKeys("New folder");

        var create_button = _driver.FindElement(By.CssSelector("[data-tid='SaveButton']"));
        create_button.Click();

        var folders = _driver.FindElement(By.CssSelector("[data-tid='Folders']"));
        Assert.That(folders.Text, Does.Contain("New folder"), "Папка не найдена");  

      }

}
