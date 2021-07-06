using System;
using System.IO;
using SimpleMonads;

namespace IoFluently
{
    public interface IHasAbsolutePath
    {
        AbsolutePath Path { get; }
        IIoService IoService => Path.IoService;
    }

    public class FileOrMissingPath : SubTypesOf<IHasAbsolutePath>.Either<File, MissingPath>, IHasAbsolutePath
    {
        public FileOrMissingPath(File item1) : base(item1)
        {
        }

        public FileOrMissingPath(MissingPath item3) : base(item3)
        {
        }

        public FileOrMissingPath(SubTypesOf<IHasAbsolutePath>.Either<File, MissingPath> other) : base(other)
        {
        }

        public FileOrMissingPath(IHasAbsolutePath item) : base(item)
        {
        }

        public File ExpectFile()
        {
            if (Item1 == default)
            {
                throw new InvalidOperationException($"Expected {Path} to be a file but it was a {Path.Type}");
            }
            return Item1;
        }

        public MissingPath ExpectMissingPath()
        {
            if (Item2 == default)
            {
                throw new InvalidOperationException($"Expected {Path} to be a missing path but it was a {Path.Type}");
            }
            return Item2;
        }

        public AbsolutePath Path => Value.Path;
    }
    
    public class FolderOrMissingPath : SubTypesOf<IHasAbsolutePath>.Either<Folder, MissingPath>, IHasAbsolutePath
    {
        public FolderOrMissingPath(Folder item1) : base(item1)
        {
        }

        public FolderOrMissingPath(MissingPath item3) : base(item3)
        {
        }

        public FolderOrMissingPath(SubTypesOf<IHasAbsolutePath>.Either<Folder, MissingPath> other) : base(other)
        {
        }

        public FolderOrMissingPath(IHasAbsolutePath item) : base(item)
        {
        }

        public Folder ExpectFoldre()
        {
            if (Item1 == default)
            {
                throw new InvalidOperationException($"Expected {Path} to be a folder but it was a {Path.Type}");
            }
            return Item1;
        }

        public MissingPath ExpectMissingPath()
        {
            if (Item2 == default)
            {
                throw new InvalidOperationException($"Expected {Path} to be a missing path but it was a {Path.Type}");
            }
            return Item2;
        }

        public AbsolutePath Path => Value.Path;
    }
    
    public class File : IHasAbsolutePath
    {
        public File(AbsolutePath path)
        {
            Path = path;
            if (!Path.IsFile)
            {
                throw new IOException($"{Path} is not a file");
            }
        }

        public AbsolutePath Path { get; }
    }

    public class Folder : IHasAbsolutePath
    {
        public AbsolutePath Path { get; }

        public Folder(AbsolutePath path)
        {
            Path = path;
        }
    }

    public class MissingPath : IHasAbsolutePath
    {
        public AbsolutePath Path { get; }

        public MissingPath(AbsolutePath path)
        {
            Path = path;
            if (Path.Exists)
            {
                throw new IOException($"{Path} should not exist");
            }
        }
    }
}