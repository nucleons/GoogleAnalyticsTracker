using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using GoogleAnalyticsTracker.Core.v1;
using GoogleAnalyticsTracker.WebAPI2.v1.Interface;

namespace GoogleAnalyticsTracker.WebAPI2.v1
{
    public class ActionTrackingAttribute : AsyncActionFilterAttribute
    {
        Func<HttpActionContext, bool> _isTrackableAction;
        public Tracker Tracker { get; set; }

        public Func<HttpActionContext, bool> IsTrackableAction
        {
            get
            {
                if (_isTrackableAction != null)
                    return _isTrackableAction;

                return action => true;
            }
            set { _isTrackableAction = value; }
        }

        public ActionTrackingAttribute()
            : this(null, null, null, null)
        {
        }

        public ActionTrackingAttribute(string trackingAccount, string trackingDomain)
            : this(trackingAccount, trackingDomain, null, null)
        {
        }

        public ActionTrackingAttribute(string trackingAccount)
            : this(trackingAccount, null, null, null)
        {
        }

        public ActionTrackingAttribute(string trackingAccount, string trackingDomain, string actionDescription, string actionUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(trackingDomain) && System.Web.HttpContext.Current != null)
                    trackingDomain = System.Web.HttpContext.Current.Request.Url.Host;
            }
            catch { /*intended */ }

            Tracker = new Tracker(trackingAccount, trackingDomain, new CookieBasedAnalyticsSession(), new AspNetWebApiTrackerEnvironment());
            ActionDescription = actionDescription;
            ActionUrl = actionUrl;
        }

        public ActionTrackingAttribute(Tracker tracker)
            : this(tracker, action => true)
        {
        }

        public ActionTrackingAttribute(Tracker tracker, Func<HttpActionContext, bool> isTrackableAction)
        {
            Tracker = tracker;
            IsTrackableAction = isTrackableAction;
        }

        public ActionTrackingAttribute(string trackingAccount, string trackingDomain, Func<HttpActionContext, bool> isTrackableAction)
        {
            Tracker = new Tracker(trackingAccount, trackingDomain, new CookieBasedAnalyticsSession(), new AspNetWebApiTrackerEnvironment());
            IsTrackableAction = isTrackableAction;
        }

        public async override Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            if (IsTrackableAction(actionContext))
            {
                var requireRequestAndResponse = Tracker.AnalyticsSession as IRequireRequestAndResponse;
                if (requireRequestAndResponse != null)
                    requireRequestAndResponse.SetRequestAndResponse(actionContext.Request, actionContext.Response);

                await OnTrackingAction(actionContext);
            }
        }

        public virtual async Task<TrackingResult> OnTrackingAction(HttpActionContext filterContext)
        {
            return await Tracker.TrackPageViewAsync(
                filterContext.Request,
                BuildCurrentActionName(filterContext),
                BuildCurrentActionUrl(filterContext));
        }
    }
}