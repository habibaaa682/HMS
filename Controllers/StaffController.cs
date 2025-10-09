using HMS.Models;
using HMS.Models.DTO;
using HMS.Services;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Controllers
{
    public class StaffController : BaseBusinessController<Staff,StaffDto,IStaffServices>
    {
        public StaffController(IStaffServices ser)
        {
            this.ser = ser;
        }
        [HttpPost]
        public override async Task<IActionResult> Insert(StaffDto staff)
        {
            var createdGuest = await ser.Insert(staff, UserId);
            if (createdGuest == null) return BadRequest("Staff not created");
            return Ok(createdGuest);
        }
        [HttpPut]
        public override async Task<IActionResult> Edit(StaffDto staff)
        {
            var updatedGuest = await ser.Edit(staff, UserId);
            if (updatedGuest == null) return BadRequest("Staff not updated");
            return Ok(updatedGuest);
        }
        [HttpDelete]
        public override async Task<IActionResult> Remove(int staffId)
        {
            var deletedGuest = await ser.Remove(staffId, UserId);
            if (deletedGuest == false) return BadRequest("Staff not deleted");
            return Ok(deletedGuest);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllStaffs()
        {
            var guests = await ser.GetAllStaffs();
            if (guests == null) return NotFound("No Staff found");
            return Ok(guests);
        }
        [HttpGet]
        public async Task<IActionResult> GetStaffById(int staffId)
        {
            var guest = await ser.GetStaffById(staffId);
            if (guest == null) return NotFound("staff not found");
            return Ok(guest);
        }
    }
}
