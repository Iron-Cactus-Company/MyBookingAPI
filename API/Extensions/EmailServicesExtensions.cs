using API.Service.Email;

namespace API.Extensions;

public static class EmailServicesExtensions
{
    //Add extension method for adding all needed services to builder
    public static IServiceCollection AddEmailServices(this IServiceCollection services, IConfiguration configuration)
    {
        var email = configuration["Email"];
        var password = configuration["EmailPassword"];
        var smtpServer = configuration["SmtpServer"];
        
        var smtpPortStr = configuration["SmtpPort"];
        int smtpPort;
        var isPortDefinedSuccessfully = int.TryParse(smtpPortStr, out smtpPort);

        if (email is null || password is null || smtpServer is null || !isPortDefinedSuccessfully)
        {
            var notFoundVariables = "";
            if (email is null) notFoundVariables += "Email, ";
            if (password is null) notFoundVariables += "EmailPassword, ";
            if (smtpServer is null) notFoundVariables += "SmtpServer, ";
            if (!isPortDefinedSuccessfully) notFoundVariables += "SmtpPort";
            
            throw new Exception(
                    $"Could not define some of environment variables required for sending emails\n" +
                    $"Here is a list of them: [ {notFoundVariables} ]"
                );
        }
        
        var emailConfig = new EmailConfiguration
        {
            Email = configuration["Email"],
            Password = configuration["EmailPassword"],
            SmtpServer = configuration["SmtpServer"],
            Port = smtpPort
        };
        
        services.AddSingleton(emailConfig);
        services.AddScoped<IEmailSender, EmailSender>();

        return services;
    }
}