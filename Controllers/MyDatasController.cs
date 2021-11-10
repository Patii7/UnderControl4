using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UnderControl.Data;
using UnderControl.Models;

namespace UnderControl.Controllers
{
    public class MyDatasController : Controller
    {
        private readonly ApplicationDbContext _context;
        public MyDatasController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpPost] // HTTPGET pobieranie danych, HTTPPOST wysylanie ewentualnie HTTPPUT, lecz czesciej HTTPPOST
        [Route("Send")]
        public bool Send(double res, int id)
        {
            SaveResult(res, id);
            _context.Mesurements.Add(new Mesurement { DeviceId = id, Mesuremet = res });
            return true;
        }
        [HttpGet]
        public double GetLastMesurement(int DevicesId)
        {
            _context.Mesurements.Add(new Mesurement { DeviceId = 1, Mesuremet = 37.2 });
            _context.SaveChanges();
            //Expression<Func<Mesurement, bool>> predicate = (Mesurement) => false;
            //predicate = predicate.Last(n => n.DeviceId == DevicesId);

            return _context.Mesurements.AsEnumerable().Last(n => n.DeviceId == DevicesId).Mesuremet;
        }
        public string FileReader()
        {
            var filePath = "P:/Studia/Inżynierka/Data/record.txt";

            if (!System.IO.File.Exists(filePath))
                return string.Empty;

            var fileRecords = System.IO.File.ReadAllText(filePath);
            return fileRecords;
        }

        public void SaveResult(double res, int id)
        {
            var filePath = "P:/Studia/Inżynierka/Data/record.txt";
            var dir = Path.GetDirectoryName(filePath);

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            var fileRecords = FileReader();
            using (var st = new StreamWriter(filePath))
            {
                st.Write(fileRecords);
                st.Write(res + ";" + id);

                st.Flush();
                st.Close();
            }

        }
        [Authorize]
        // GET: MyDatas
        public async Task<IActionResult> Index()
        {
            return View(await _context.MyData.ToListAsync());
        }

        [Authorize]
        // GET: MyDatas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var myData = await _context.MyData
                .FirstOrDefaultAsync(m => m.ID == id);
            if (myData == null)
            {
                return NotFound();
            }

            return View(myData);
        }

        // GET: MyDatas/Create
        [Authorize]
        public IActionResult Create()
        {
           
            return View();
        }

        // POST: MyDatas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Date,Time,ID,Temperature,Feeling,Color,Consistency,Quantity,Cervix,Bleeding,Sex,Others")] MyData myData)
        {
            if (ModelState.IsValid)
            {
                _context.Add(myData);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
        
            return View(myData);
         
                          
        }

        // GET: MyDatas/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var myData = await _context.MyData.FindAsync(id);
            if (myData == null)
            {
                return NotFound();
            }
            return View(myData);
        }

        // POST: MyDatas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Date,Time,ID,Temperature,Feeling,Color,Consistency,Quantity,Cervix,Bleeding,Sex,Others")] MyData myData)
        {
            if (id != myData.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(myData);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MyDataExists(myData.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(myData);
        }

        // GET: MyDatas/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var myData = await _context.MyData
                .FirstOrDefaultAsync(m => m.ID == id);
            if (myData == null)
            {
                return NotFound();
            }

            return View(myData);
        }

        // POST: MyDatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var myData = await _context.MyData.FindAsync(id);
            _context.MyData.Remove(myData);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MyDataExists(int id)
        {
            return _context.MyData.Any(e => e.ID == id);
        }
    }
}
