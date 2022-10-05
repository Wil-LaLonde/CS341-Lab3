namespace Lab2Solution;
/// <summary>
/// Name: Wil LaLonde
/// Date: 10/05/2022
/// Description: Lab3
/// Bugs: KNOWN ISSUES:
/// (1): If an entry is too long, it will go off the screen
/// (2): Sometimes, the connection with a the database will 
///      stop as soon as trying to do something (add, delete, etc.)
///      Not sure what really causes this, maybe my computer
///      is too slow causing a timeout???
/// Bug(s) For Us To Fix:
/// During my testing, I found a few bugs with the given solution
/// which were corrected.
/// (1): I think the main bug was that the business logic/entry class 
///      (can't remember which one) was checking the wrong difficulty
///      value. This was updated to correct that issue.
/// (2): When clicking the edit entry button without an option selected,
///      the entire application would crash because of a NullPointerException
/// (3): When deleting/editing an entry without anything selected, the wrong
///      error message would show. For some reason, the entry selection would
///      not clear unless setting it to null (this would be after deleting/editing
///      an entry).
/// (4): I'm pretty sure date was not being checked. Added a date check.
/// Reflection: This lab was pretty fun. I personally enjoy SQL a lot and
///             being able to connect VS and a database was exciting. I
///             did run into some strange issues where the database would
///             immediately just timeout on me. So not really sure what 
///             that is all about. I'm guessing my computer was just struggling
///             to make the request since it already has a hard enough
///             time running the emulator and .NET MAUI at the same time.
///             When working in the lab, everything worked much better.
/// </summary>
public static class MauiProgram {
    public static IBusinessLogic ibl = new BusinessLogic();

    public static MauiApp CreateMauiApp() {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts => {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        return builder.Build();
    }
}
