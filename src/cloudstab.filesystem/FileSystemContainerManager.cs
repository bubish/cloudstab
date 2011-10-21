/*
 Copyright (c) 2011, Andrew Benz
 All rights reserved.
 
 Redistribution and use in source and binary forms, with or without 
 modification, are permitted provided that the following conditions are met:
 
 Redistributions of source code must retain the above copyright notice, this 
 list of conditions and the following disclaimer.
 Redistributions in binary form must reproduce the above copyright notice, 
 this list of conditions and the following disclaimer in the documentation 
 and/or other materials provided with the distribution.
 THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
 AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
 IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
 ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE 
 LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
 CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
 SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
 INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
 CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
 ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF 
 THE POSSIBILITY OF SUCH DAMAGE.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SystemWrapper.IO;
using cloudstab.core;

namespace cloudstab.filesystem {
  public class FileSystemContainerManager : IBlobContainerManager {
    private readonly string _rootPath;
    
    public FileSystemContainerManager(string rootPath) : this(rootPath, new DirectoryWrap()) { }

    public FileSystemContainerManager(string rootPath, IDirectoryWrap directory) {
      _rootPath = rootPath;
      _directoryWrapper = directory;

      if (!_directoryWrapper.Exists(rootPath)) {
        _directoryWrapper.CreateDirectory(rootPath);  
      }
    }

    private readonly IDirectoryWrap _directoryWrapper;

    #region "IBlobContainerManager Implementation"
    public IEnumerable<IBlobContainer> List() {
      return _directoryWrapper.GetDirectories(_rootPath)
        .Select(x => new FileSystemContainer(x));
    }

    public IBlobContainer Create(string name) {
      EnsureNameIsValid(name);
      
      _directoryWrapper.CreateDirectory(name);
      return new FileSystemContainer(name);
    }

    public IBlobContainer Get(string name) {
      throw new NotImplementedException();
    }

    public void Delete(string name) {
      EnsureNameIsValid(name);

      if (_directoryWrapper.Exists(Path.Combine(_rootPath, name))) {
        _directoryWrapper.Delete(name, true);
      }
    }

    private static void EnsureNameIsValid(string name) {
      if (string.IsNullOrWhiteSpace(name)) {
        throw new ArgumentException("Container name cannot be empty.", "name");
      }
    }
    #endregion
  }
}

