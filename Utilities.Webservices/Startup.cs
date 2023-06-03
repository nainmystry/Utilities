using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace Utilities.Webservices
{
    public class Startup
    {
        public IConfiguration configuration { get; set; }
        public Startup(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            services.AddControllers();
            services.AddControllersWithViews();
            services.AddHttpClient();

            //Add Swagger gen to services, make sure we have Swashbuckle.AspNetCore package
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "API", Version = "v1" });
            });


            //Add api versioning, make sure we have Microsoft.AspNetCore.Mvc.Versioning package installed
            //After adding below add apiversion attribute on the controller level as well. For reference, check TestController.
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);

                //whether to report API version in response or not
                options.ReportApiVersions = true;

                //specify the versioning scheme
                options.ApiVersionReader = new HeaderApiVersionReader("api-version");

            });

        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configure the HTTP request pipeline.
            if (!env.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.Use((context, next) =>
            {
                //These headers "X-AspNet-Version" and "X-AspNetMvc-Version" reveal the version of the ASP.NET and ASP.NET MVC frameworks respectively.
                //Removing these headers helps to hide specific framework information, which can be useful for security purposes
                //as it makes it harder for potential attackers to exploit known vulnerabilities associated with specific versions
                context.Response.Headers.Remove("X-AspNet-Version");
                context.Response.Headers.Remove("X-AspNetMvc-Version");

                //This header helps to prevent clickjacking attacks by specifying whether a web page can be embedded within a frame or iframe.
                //The value "SAMEORIGIN" allows the page to be framed only by pages from the same origin, providing some protection against clickjacking attacks.
                context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");

                //It defines the allowed sources for various types of content, such as scripts, stylesheets, images, and more.
                //The policy mentioned in the code snippet allows scripts and resources from the same origin and also allows 'unsafe-eval' and 'unsafe-inline'.
                //It also sets some additional security measures like blocking mixed content and upgrading insecure requests.
                context.Response.Headers.Add("Content-Security-Policy", "default-src 'self' 'unsafe-eval' 'unsafe-inline' *; script-src 'self' 'unsafe-inline' 'unsafe-eval'; img-src 'self' * data:; connect-src 'self' *; upgrade-insecure-requests; block-all-mixed-content");

                //This header enables HTTP Strict Transport Security (HSTS) and informs the browser that future requests should only be made over HTTPS
                //for a specified period (in this case, one year). HSTS helps to enforce secure connections and mitigate against man-in-the-middle attacks and protocol downgrade attacks
                context.Response.Headers.Add("Strict-Transport-Security", "max-age=31536000; includeSubDomains");

                //This header enables the browser's built-in cross-site scripting (XSS) protection.
                //The value "1; mode=block" instructs the browser to block the rendering of the page if an XSS attack is detected
                context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");

                //This header helps to prevent MIME type sniffing, where the browser tries to determine the content type of a response based on its content. 
                //The value "nosniff" tells the browser to strictly respect the declared content type and not perform MIME sniffing.
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");

                //This header controls how much referrer information should be included in the request headers when navigating from one page to another.
                //"no-referrer" specifies that no referrer information should be sent.
                context.Response.Headers.Add("Referrer-Policy", "no-referrer");

                //This header specifies the permissions or restrictions for certain browser features or APIs used by the web page.
                //The value "self" allows the web page to use features from the same origin.
                context.Response.Headers.Add("Feature-Policy", "self");

                //This header specifies the server software being used. Setting it to an empty value (" ") effectively removes the server information from the response,
                //making it harder for potential attackers to gather information about the server and its vulnerabilities.
                context.Response.Headers["Server"] = " ";

                //This header typically contains the path or location of the source files on the server.
                //Setting it to an empty value (" ") removes this information from the response, helping to obscure internal file structure.
                context.Response.Headers["X-SourceFiles"] = " ";

                //his header indicates the technology or framework powering the web application.
                //Removing this header helps to hide specific framework information and adds an extra layer of security through obfuscation.
                context.Response.Headers.Remove("X-Powered-By");

                //This header controls caching behavior. Setting it to "no-store" instructs the browser and
                //intermediate caches not to store a cached copy of the response, ensuring that the content is always fetched from the server.
                context.Response.Headers.Add("Cache-Control", "no-store");
                return next();
            });          

            // can either allow all
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            //or can allow defined origins
            //MyAllowSpecificOrigins = ["https://otherexample.com", "https://oneexample.com"]
            //app.UseCors(MyAllowSpecificOrigins);

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();
            app.UseHttpsRedirection();

            //app.MapRazorPages();

            //app.UseMiddleware<MiddleWareClassName>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                   name: "default",
                  pattern: "{controller=Test}/{action=Get}/{id?}"
                  );
            });

            //Add swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API verison v1");
            });



        }


    }
}
