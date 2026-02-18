using System.Threading.Tasks;
using log4net;

public class ProcessorService
{
    private static readonly ILog log = LogManager.GetLogger(typeof(ProcessorService));

    public async Task Run()
    {
        var dataService = new DataService();
        var fileService = new FileService();

        var data = dataService.GetData();

        await Task.Run(() =>
        {
            Parallel.ForEach(data, item =>
            {
                log.Info("Processing: " + item);
                fileService.Write(item);
            });
        });
    }
}
