using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Vue.Data;
namespace it_template.Areas.V1.Controllers
{
	[Area("V1")]
	//[Authorize(Roles = "Administrator")]
	 [Authorize]
	public class BaseController : Controller
	{
		protected readonly ItContext _context;

		public BaseController(ItContext context)
		{
			_context = context;
		}
	}
}
