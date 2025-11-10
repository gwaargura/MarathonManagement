using System.ComponentModel.DataAnnotations;

namespace MM_client.Models
{
    public class CreateMarathonViewModel
    {
        [Required(ErrorMessage = "Tên Marathon không được để trống.")]
        public string MarathonName { get; set; } = null!;

        public string? Description { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa điểm.")]
        public string? Location { get; set; }

        [Required(ErrorMessage = "Ngày bắt đầu là bắt buộc.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Ngày kết thúc là bắt buộc.")]
        public DateTime EndDate { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Phí đăng ký phải là số hợp lệ.")]
        public decimal RegistrationFee { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Số lượng người tham gia tối đa phải lớn hơn 0.")]
        public int? MaxParticipants { get; set; }

        public string? Status { get; set; }

        // Checkpoints chosen on the map
        // Hidden field from map
        public string? CheckpointsJson { get; set; }
    }

    public class CreateCheckpointViewModel
    {
        public string? Name { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int Sequence { get; set; }
    }
}
