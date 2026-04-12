using System.Text.RegularExpressions;

namespace MauiLearn.MultiApp.Interfaces
{
    /* EXAMPLE:
     * ValidationMessage: -> shows the IsNotNullOrEmptyRule<T> validation rule, which is used to 
perform validation of the username and password entered by the user on the LoginView when using 
mock services
      * the following code below shows a validation 
rule for validating email addresses: EmailRule<T>
     */

    public interface IValidationRule<T>
    {
        string ValidationMessage { get; set; }
        bool Check(T value);
    }

    public class IsNotNullOrEmptyRule<T> : IValidationRule<T>
    {
        
        public string ValidationMessage { get; set; }
        /*
          * The Check method returns a boolean indicating whether the value argument is null, empty, or consists 
 only of whitespace characters. 
          */
        public bool Check(T value) =>
            value is string str && !string.IsNullOrWhiteSpace(str);
    }

    public class EmailRule<T> : IValidationRule<T>
    {
        private readonly Regex _regex = new(@"^([w.-]+)@([w-]+)((.(w){2,3})+)$");

        public string ValidationMessage { get; set; }

        public bool Check(T value) =>
            value is string str && _regex.IsMatch(str);
    }
}