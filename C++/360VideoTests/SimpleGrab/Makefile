CXX = g++

CXXFLAGS := -Wall -pthread -fPIC -O2 -std=c++14
LDFLAGS := -Wl,--exclude-libs=ALL

OUTPUT_EXE = LadybugSimpleGrab

# Include path
LADYBUG_API_INCLUDE = -I../../include -I/usr/include/ladybug
ALL_INCLUDE = ${LADYBUG_API_INCLUDE}

# Lib path
LADYBUG_LIB = -L../../lib -L/usr/lib/ladybug -lflycapture -lladybug -lptgreyvideoencoder
ALL_LIBS = ${LADYBUG_LIB}

OBJDIR = obj

ALL_CPP_FILES := $(wildcard *.cpp)
CPP_FILES := $(ALL_CPP_FILES)
OBJ_FILES := $(addprefix $(OBJDIR)/,$(notdir $(CPP_FILES:.cpp=.o)))

all: ${OUTPUT_EXE}

${OUTPUT_EXE}: make_obj_dir ${OBJ_FILES}
	@echo Creating executable
	${CXX} ${LDFLAGS} -o ${OUTPUT_EXE} ${OBJ_FILES} ${ALL_LIBS}
	@strip --strip-unneeded ${OUTPUT_EXE}
	@cp $(OUTPUT_EXE) ../../bin
	
obj/%.o: %.cpp
	${CXX} ${CXXFLAGS} ${ALL_INCLUDE} -c $< -o $@
	
make_obj_dir:
	@mkdir -p $(OBJDIR)

clean_obj:
	@rm -rf obj ${OBJ_FILES} $../../bin/${OUTPUT_EXE}

clean: clean_obj
