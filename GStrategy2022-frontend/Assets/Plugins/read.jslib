mergeInto(LibraryManager.library, {
    ReadGameHistory: function () {
      debugger;
      const str =  window.reportString
      const buffer = _malloc(lengthBytesUTF8(str)+1)
      writeStringToMemory(str,buffer)
      return buffer
  }
});