using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VFS.Server.Core.FS;
using VFS.Server.Core.FS.Impl;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace VFS.Server.Core.Commands.Impl
{
    internal class COPYCommand : BaseCommand
    {
        #region ICommand Members

        public override void Execute(CommandContext context)
        {
            if (!hasTwoParameters(context.Args))
            {
                throw new ArgumentException("command args");
            }
            string sourcePath = context.Args[0];
            string destPath = context.Args[1];

            IDirectory destDirectory = _fsManager.FindDirectory(destPath, context.User.CurrentDirectory);
            IDirectory sourceDirectory = _fsManager.FindDirectory(sourcePath, context.User.CurrentDirectory);
            if (sourceDirectory == null)
            {
                IFile sourceFile = _fsManager.FindFile(sourcePath, context.User.CurrentDirectory);
                IFile copiedFile = (IFile)DeepCopy(sourceFile);
                destDirectory.AddFile(copiedFile);
            }
            else
            {                
                if (sourceDirectory.Parent == null)
                {
                    throw new IOException();
                }
                IDirectory copiedDirectory = (IDirectory)DeepCopy(sourceDirectory);
                destDirectory.AddDirectory(copiedDirectory);
            }            
        }

        #endregion

        /// <summary>
        /// Выполняет глубокое копирование объекта
        /// </summary>
        /// <param name="current">Объект для копирования</param>
        ///
        /// <exception cref="ArgumentNullException">если <paramref name="current"/> = null</exception>
        /// <exception cref="System.Runtime.Serialization.SerializationException">При сериализации или десериализации <paramref name="current"/></exception>
        public static object DeepCopy(object current)
        {
            var stream = new MemoryStream();
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, current);
            stream.Seek(0, SeekOrigin.Begin);
            object copy = formatter.Deserialize(stream);
            stream.Close();
            return copy;
        }
    }
}
