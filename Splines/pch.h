// pch.h: это предварительно скомпилированный заголовочный файл.
// Перечисленные ниже файлы компилируются только один раз, что ускоряет последующие сборки.
// Это также влияет на работу IntelliSense, включая многие функции просмотра и завершения кода.
// Однако изменение любого из приведенных здесь файлов между операциями сборки приведет к повторной компиляции всех(!) этих файлов.
// Не добавляйте сюда файлы, которые планируете часто изменять, так как в этом случае выигрыша в производительности не будет.

#ifndef PCH_H
#define PCH_H

// Добавьте сюда заголовочные файлы для предварительной компиляции
#include "framework.h"
#include "mkl.h"
#include "mkl_df_types.h"

extern "C" _declspec(dllexport)
double Interpolate(
    const int length, const double* points,
    const double* func, double* res,
    double* der, int gridlen, double* grid,
    const double* limits, double* integrals
);

extern "C" _declspec(dllexport)
double Integrate(
    const int length, const double* points,
    const double* func, const double* limits,
    double* integrals
);





#endif //PCH_H
