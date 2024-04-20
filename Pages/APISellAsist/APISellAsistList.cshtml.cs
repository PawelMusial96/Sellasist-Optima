using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Sellasist_Optima.Areas.Identity.Data;
using Sellasist_Optima.Models;

namespace Sellasist_Optima.Pages.APISellAsist
{
    public class APISellAsistListModel : PageModel
    {
        private readonly Sellasist_Optima.Areas.Identity.Data.Sellasist_OptimaContext _context;

        public APISellAsistListModel(Sellasist_Optima.Areas.Identity.Data.Sellasist_OptimaContext context)
        {
            _context = context;
        }

        public IList<SellAsistAPI> SellAsistAPI { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.SellAsistAPI != null)
            {
                SellAsistAPI = await _context.SellAsistAPI.ToListAsync();
            }
        }
    }
}
