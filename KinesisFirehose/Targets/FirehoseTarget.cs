using System.Text;
using Amazon.KinesisFirehose;
using Amazon.KinesisFirehose.Model;
using NLog;
using NLog.Common;
using NLog.Config;
using NLog.Targets;

namespace KinesisFirehose.Targets;

[Target("Firehose")]
public class FirehoseTarget : TargetWithLayout
{
    private AmazonKinesisFirehoseClient _client;

    [RequiredParameter]
    public string DeliveryStreamName { get; set; }

    [RequiredParameter]
    public string Region { get; set; }

    protected override void InitializeTarget()
    {
        base.InitializeTarget();
        var credentials = new AwsCredentialService().GetCredentialsAsync();
        var region = Amazon.RegionEndpoint.GetBySystemName(Region);
        _client = new AmazonKinesisFirehoseClient(credentials, region);
    }

    protected override void Write(LogEventInfo logEvent)
    {
        var message = this.Layout.Render(logEvent);
        var record = new Record { Data = new System.IO.MemoryStream(Encoding.UTF8.GetBytes(message + "\n")) };
        var request = new PutRecordRequest { DeliveryStreamName = DeliveryStreamName, Record = record };
        _client.PutRecordAsync(request)
            .ContinueWith(t =>
            {
                if (t.Exception != null)
                {
                    InternalLogger.Error(t.Exception, "Firehose PutRecord failed");
                }
            });
    }
}
