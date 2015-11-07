#ifndef __COZY_ELF_OBJECT__
#define __COZY_ELF_OBJECT__

#include "ElfDef.h"
#include "ElfEnum.h"
#include "ElfStructs.h"
#include "ElfProgramHeaderObject.h"
#include <vector>
#include <memory>

namespace CozyElf
{
    class COZY_API ElfObject
    {
    public:
        ElfObject(std::shared_ptr<Elf32> obj);
        ~ElfObject();

        ElfClass GetElfClass() const;
        Endianess GetEndian() const;
        ElfFileType GetFileType() const;
        MachineType GetMachineType() const;
        Elf32_Word GetElfVersion() const;
        Elf32_Addr GetEntry() const;

    private:
        void Init();

    private:
        std::vector<ElfProgramHeaderObject>     m_PhdrObjs;
        std::shared_ptr<Elf32>                  m_obj;
    };
}


#endif // __COZY_ELF_OBJECT__