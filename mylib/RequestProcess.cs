using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public static class RequestProcess
{
    public static async Task<string> FormProcess(HttpRequest request)
    {
        //Xử lý đọc dữ liệu Form - khi post - dữ liệu này trình  bày trên Form
        string hovaten = "";
        bool luachon = false;
        string email = "";
        string password = "";
        string thongbao = "";

        // Đọc dữ liệu từ Form do truy vấn gửi đến (chỉ xử lý khi là post)
        if (request.Method == "POST")
        {
            IFormCollection _form = request.Form;

            email = _form["email"].FirstOrDefault() ?? "";
            hovaten = _form["hovaten"].FirstOrDefault() ?? "";
            password = _form["password"].FirstOrDefault() ?? "";
            luachon = (_form["luachon"].FirstOrDefault() == "on");

            thongbao = $@"Dữ liệu post - email: {email}
                          - hovaten: {hovaten} - password: {password}
                          - luachon: {luachon} ";


            // var filePath = Path.GetTempFileName();
            // Xử lý nếu có file upload (hình ảnh,  ... )
            if (_form.Files.Count > 0)
            {
                string thongbaofile = "Các file đã upload: ";
                foreach (IFormFile formFile in _form.Files)
                {
                    if (formFile.Length > 0)
                    {
                        var filePath = "wwwroot/upload/" + formFile.FileName;    // Lấy tên  file
                        if (!Directory.Exists("wwwroot/upload/")) Directory.CreateDirectory("wwwroot/upload/");
                        thongbaofile += $"{filePath} {formFile.Length} bytes";
                        using (var stream = new FileStream(filePath, FileMode.Create)) // Mở stream để lưu file, lưu file ở thư mục wwwroot/upload/
                        {
                            await formFile.CopyToAsync(stream);
                        }
                    }

                }
                thongbao += "<br>" + thongbaofile;
            }

        }
        string format = await File.ReadAllTextAsync("formtest.html");   // Đọc nội dung HTML từ file
        string formhtml = string.Format(format, hovaten, email, luachon ? "checked" : "");
        return (formhtml + thongbao).HtmlTag("div", "container mt-4");
    }
    public static string RequestInfo(HttpRequest request)
    {

        var sb = new StringBuilder();

        // Lấy http scheme (http|https)
        var scheme = request.Scheme;
        sb.Append(("scheme".td() + scheme.td()).tr());

        // HOST Header
        var host = (request.Host.HasValue ? request.Host.Value : "no host");
        sb.Append(("host".td() + host.td()).tr());


        // Lấy pathbase (URL Path - cho Map)
        var pathbase = request.PathBase.ToString();
        sb.Append(("pathbase".td() + pathbase.td()).tr());

        // Lấy Path (URL Path)
        var path = request.Path.ToString();
        sb.Append(("path".td() + path.td()).tr());

        // Lấy chuỗi query của URL
        var QueryString = request.QueryString.HasValue ? request.QueryString.Value : "no query string";
        sb.Append(("QueryString".td() + QueryString.td()).tr());

        // Lấy phương thức
        var method = request.Method;
        sb.Append(("Method".td() + method.td()).tr());

        // Lấy giao thức
        var Protocol = request.Protocol;
        sb.Append(("Protocol".td() + Protocol.td()).tr());

        // Lấy ContentType
        var ContentType = request.ContentType;
        sb.Append(("ContentType".td() + ContentType.td()).tr());

        // Lấy danh sách các Header và giá trị  của nó, dùng Linq để lấy
        // Header gửi đến lưu trong thuộc tính Header  kiểu Dictionary
        var listheaderString = request.Headers.Select((header) => $"{header.Key}: {header.Value}".HtmlTag("li"));
        var headerhmtl = string.Join("", listheaderString).HtmlTag("ul"); // nối danh sách thành 1
        sb.Append(("Header".td() + headerhmtl.td()).tr());

        // Lấy danh sách các Header và giá trị  của nó, dùng Linq để lấy
        var listcokie = request.Cookies.Select((header) => $"{header.Key}: {header.Value}".HtmlTag("li"));
        var cockiesHtml = string.Join("", listcokie).HtmlTag("ul");
        sb.Append(("Cookies".td() + cockiesHtml.td()).tr());


        // Lấy tên và giá trí query
        var listquery = request.Query.Select((header) => $"{header.Key}: {header.Value}".HtmlTag("li"));
        var queryhtml = string.Join("", listquery).HtmlTag("ul");
        sb.Append(("Các Query".td() + queryhtml.td()).tr());

        //Kiểm tra thử query tên abc có không
        Microsoft.Extensions.Primitives.StringValues abc;
        bool existabc = request.Query.TryGetValue("abc", out abc);
        string queryVal = existabc ? abc.FirstOrDefault() : "không có giá trị";
        sb.Append(("abc query".td() + queryVal.ToString().td()).tr());

        string info = "Thông tin Request".HtmlTag("h2") + sb.ToString().HtmlTag("table", "table table-sm table-bordered");
        return info;
    }
}