//利用文件头判断文件类型
//上传文件时经常需要做文件类型判断，例如图片、文档等，普通做法是直接判断文件后缀名，而文艺青年为了防止各种攻击同时也会加上使用文件头信息判断文件类型。

//原理很简单：用文件头判断,直接读取文件的前2个字节即可。

//Demo
using System;
using System.IO;
 
namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            FileStream fs = new FileStream(@"C:\test.rar", FileMode.Open, FileAccess.Read);
            BinaryReader reader = new BinaryReader(fs);//转换为二进制流
            byte[] buff = new byte[2];
            string result = string.Empty;
            try
            {
                fs.Read(buff, 0, 2);
                result = buff[0].ToString() + buff[1].ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
 
            reader.Close();
            fs.Close();
 
            if (result == "8297")//rar:8297
            {
                Console.WriteLine("文件格式是rar." + result);
            }
            else
            {
                Console.WriteLine("文件格式不是rar." + result);
            }
            Console.ReadLine();
        }
    }
}
 
 /*

附常见文件的文件头信息
常见文件的文件头（十进制）

jpg: 255,216

gif: 71,73

bmp: 66,77

png: 137,80

doc: 208,207

docx: 80,75

xls: 208,207

xlsx: 80,75

js: 239,187

swf: 67,87

txt: 70,67

mp3: 73,68

wma: 48,38

mid: 77,84

rar: 82,97

zip: 80,75

xml: 60,63


常用文件的文件头如下(16进制)：

JPEG (jpg)，文件头：FFD8FF

PNG (png)，文件头：89504E47

GIF (gif)，文件头：47494638

TIFF (tif)，文件头：49492A00

Windows Bitmap (bmp)，文件头：424D

CAD (dwg)，文件头：41433130

Adobe Photoshop (psd)，文件头：38425053

Rich Text Format (rtf)，文件头：7B5C727466

XML (xml)，文件头：3C3F786D6C

HTML (html)，文件头：68746D6C3E

Email [thorough only] (eml)，文件头：44656C69766572792D646174653A

Outlook Express (dbx)，文件头：CFAD12FEC5FD746F

Outlook (pst)，文件头：2142444E

MS Word/Excel (xls.or.doc)，文件头：D0CF11E0

MS Access (mdb)，文件头：5374616E64617264204A

WordPerfect (wpd)，文件头：FF575043

Postscript (eps.or.ps)，文件头：252150532D41646F6265

Adobe Acrobat (pdf)，文件头：255044462D312E

Quicken (qdf)，文件头：AC9EBD8F

Windows Password (pwl)，文件头：E3828596

ZIP Archive (zip)，文件头：504B0304

RAR Archive (rar)，文件头：52617221

Wave (wav)，文件头：57415645

AVI (avi)，文件头：41564920

Real Audio (ram)，文件头：2E7261FD

Real Media (rm)，文件头：2E524D46

MPEG (mpg)，文件头：000001BA

MPEG (mpg)，文件头：000001B3

Quicktime (mov)，文件头：6D6F6F76

Windows Media (asf)，文件头：3026B2758E66CF11

MIDI (mid)，文件头：4D546864
*/