using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompareBazaar.Models
{

    public class Product
    {
        public int count { get; set; }
        public object next { get; set; }
        public object previous { get; set; }
        public Result[] results { get; set; }
    }

    public class Result
    {
        public int id { get; set; }
        public string name { get; set; }
        public float price { get; set; }
        public float discount { get; set; }
        public string color { get; set; }
        public string url { get; set; }
        public Product_Images product_images { get; set; }
        public Availability availability { get; set; }
        public Specifications specifications { get; set; }
        public Brand brand { get; set; }
        public Vendor vendor { get; set; }
    }

    public class Product_Images
    {
        public int id { get; set; }
        public string image { get; set; }
    }

    public class Availability
    {
        public int id { get; set; }
        public bool upcoming { get; set; }
        public object upcoming_date { get; set; }
        public bool out_of_stock { get; set; }
    }

    public class Specifications
    {
        public int id { get; set; }
        public General general { get; set; }
        public Display display { get; set; }
        public Memory memory { get; set; }
        public Camera camera { get; set; }
        public Video_Recording video_recording { get; set; }
        public Connectivity connectivity { get; set; }
        public Os os { get; set; }
        public Processor processor { get; set; }
        public Battery battery { get; set; }
        public Sound sound { get; set; }
        public Body body { get; set; }
    }

    public class General
    {
        public int id { get; set; }
        public string network { get; set; }
        public string sim_type { get; set; }
        public bool dual_sim { get; set; }
        public bool touch_screen { get; set; }
        public bool fingerprint_unlock { get; set; }
        public bool face_lock { get; set; }
    }

    public class Display
    {
        public int id { get; set; }
        public string screen_size { get; set; }
        public string screen_resolution { get; set; }
        public string display_type { get; set; }
    }

    public class Memory
    {
        public int id { get; set; }
        public float internal_memory { get; set; }
        public float? external_memory { get; set; }
        public float ram { get; set; }
    }

    public class Camera
    {
        public int id { get; set; }
        public string rear_camera { get; set; }
        public bool rear_camera_flash { get; set; }
        public string front_camera { get; set; }
        public bool front_camera_flash { get; set; }
    }

    public class Video_Recording
    {
        public int id { get; set; }
        public string rear_camera_video_quality { get; set; }
    }

    public class Connectivity
    {
        public int id { get; set; }
        public bool wifi { get; set; }
        public bool radio { get; set; }
        public bool infrared { get; set; }
        public bool gprs { get; set; }
        public bool edge { get; set; }
        public bool nfc { get; set; }
        public bool usb { get; set; }
        public string bluetooth_version { get; set; }
    }

    public class Os
    {
        public int id { get; set; }
        public string os { get; set; }
        public string os_version { get; set; }
    }

    public class Processor
    {
        public int id { get; set; }
        public string processor_type { get; set; }
        public string processor_speed { get; set; }
        public string chipset_group { get; set; }
        public string chipset_details { get; set; }
    }

    public class Battery
    {
        public int id { get; set; }
        public string battery_type { get; set; }
        public string battery_capacity { get; set; }
        public string stand_by_time { get; set; }
        public string talktime { get; set; }
    }

    public class Sound
    {
        public int id { get; set; }
        public bool jack_35mm { get; set; }
        public bool loudspeaker { get; set; }
    }

    public class Body
    {
        public int id { get; set; }
        public string body_height { get; set; }
        public string body_width { get; set; }
        public string body_thickness { get; set; }
        public string body_weight { get; set; }
        public string body_type { get; set; }
    }

    public class Brand
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Vendor
    {
        public int id { get; set; }
        public string name { get; set; }
    }

}
