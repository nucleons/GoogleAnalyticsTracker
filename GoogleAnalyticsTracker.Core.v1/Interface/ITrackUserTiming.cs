using System.Threading.Tasks;
using GoogleAnalyticsTracker.Core.v1.TrackerParameters;

namespace GoogleAnalyticsTracker.Core.v1.Interface
{
    public interface ITrackUserTiming
    {
        Task<TrackingResult> TrackUserTimingAsync(UserTimings userTimings);
    }
}