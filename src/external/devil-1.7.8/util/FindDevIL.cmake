if(NOT DevIL_PACKAGE_DIR)
	message("ERROR: DevIL_PACKAGE_DIR requires to be set")
endif(NOT DevIL_PACKAGE_DIR)

if(MSVC AND NOT CMAKE_CL_64)
	set(BINARY_DIRECTORY "win32-vc")
endif(MSVC AND NOT CMAKE_CL_64)

if(MSVC AND CMAKE_CL_64)
	set(BINARY_DIRECTORY "win64-vc")
endif(MSVC AND CMAKE_CL_64)

set(DevIL_INCLUDE_DIR ${DevIL_PACKAGE_DIR}/include)
set(DevIL_LIBRARY_DIR ${DevIL_PACKAGE_DIR}/lib/${BINARY_DIRECTORY})

if(WIN32)
	set(DevIL_LIBRARY DevIL ILU ILUT)
elseif(UNIX)
	set(DevIL_LIBRARY IL ILU ILUT)
endif(WIN32)

include_directories(${DevIL_INCLUDE_DIR})
link_directories(${DevIL_LIBRARY_DIR})

if(DevIL_LIBRARY AND DevIL_LIBRARY_DIR AND DevIL_INCLUDE_DIR)
	set(DevIL_FOUND "YES")
endif()
