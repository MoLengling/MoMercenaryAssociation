using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using MoMercenaryAssociation.Exceptions;

namespace MoMercenaryAssociation
{
    class MoFileSystemManager : MoSingletonBase<MoFileSystemManager>
    {
        //必须使用/或\号结尾才认为是文件夹
        public bool IsDirectory(string path)
        {
            return path.EndsWith("/") || path.EndsWith("\\");
        }

        public bool Exists(string path)
        {
            path = path.Trim();
            if (IsDirectory(path))
            {
                //是一个文件夹路径
                return Directory.Exists(path);
            }
            else
            {
                return File.Exists(path);
            }
        }

        public bool CreatePath(string path)
        {
            if (IsDirectory(path))
            {
                Directory.CreateDirectory(path);
                if (Exists(path))
                    return true;
            }
            else
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                if(Exists(Path.GetDirectoryName(path)+"/"))
                {
                    if(!Exists(path))
                    {
                        File.Create(path).Close();
                    }
                    if (Exists(path))
                        return true;
                }
            }
            return false;
        }
    } 
}
