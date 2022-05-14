// dllmain.cpp : Определяет точку входа для приложения DLL.
#include "pch.h"



double Interpolate(
    const int length, const double* points, 
    const double* func, double* res, 
    double* der, int gridlen, double* grid, 
    const double* limits, double* integrals
)
{
    DFTaskPtr task;
    int i = 3;
    double* scoeff = new double[(length - 1) * DF_PP_CUBIC];
    MKL_INT dorder[] = { 1, 1, 1 };
    double* limit1 = new double[1];
    double* limit2 = new double[1];
    limit1[0] = limits[0];
    limit2[0] = limits[1];

    int status = dfdNewTask1D(&task, length, points, DF_NON_UNIFORM_PARTITION, 1, func, DF_NO_HINT);
    if (status != DF_STATUS_OK)
    {
        return 1;
    }

    status = dfdEditPPSpline1D(task, DF_PP_CUBIC, DF_PP_NATURAL, DF_BC_1ST_LEFT_DER + DF_BC_1ST_RIGHT_DER, der, DF_NO_IC, NULL, scoeff, DF_NO_HINT);
    if (status != DF_STATUS_OK)
    {
        return 2;
    }

    status = dfdConstruct1D(task, DF_PP_SPLINE, DF_METHOD_STD);
    if (status != DF_STATUS_OK)
    {
        return status;
    }

    status = dfdInterpolate1D(task, DF_INTERP, DF_METHOD_PP, gridlen, grid, DF_UNIFORM_PARTITION, i, dorder, NULL, res, DF_NO_HINT, NULL);
    if (status != DF_STATUS_OK)
    {
        return 4;
    }

    status = dfdIntegrate1D(task, DF_METHOD_PP, 1, limit1, DF_UNIFORM_PARTITION, limit2, DF_UNIFORM_PARTITION, NULL, NULL, integrals, DF_NO_HINT);
    if (status != DF_STATUS_OK)
    {
        return status + 0.4;
    }

    status = dfDeleteTask(&task);
    if (status != DF_STATUS_OK)
    {
        return 5;
    }

    return DF_BC_1ST_LEFT_DER + DF_BC_1ST_RIGHT_DER;
}


double Integrate(
    const int length, const double* points, 
    const double* func, const double* limits, 
    double* integrals
)
{
    DFTaskPtr task;
    double* scoeff = new double[(length - 1) * DF_PP_CUBIC];
    double* limit1 = new double[1];
    double* limit2 = new double[1];
    limit1[0] = limits[0];
    limit2[0] = limits[1];

    int status = dfdNewTask1D(&task, length, points, DF_NON_UNIFORM_PARTITION, 1, func, DF_NO_HINT);
    if (status != DF_STATUS_OK)
    {
        return status - 0.1;
    }

    status = dfdEditPPSpline1D(task, DF_PP_CUBIC, DF_PP_NATURAL, DF_BC_FREE_END, NULL, DF_NO_IC, NULL, scoeff, DF_NO_HINT);
    if (status != DF_STATUS_OK)
    {
        return status - 0.2;
    }

    status = dfdConstruct1D(task, DF_PP_SPLINE, DF_METHOD_STD);
    if (status != DF_STATUS_OK)
    {
        return status - 0.3;
    }

    status = dfdIntegrate1D(task, DF_METHOD_PP, 1, limit1, DF_UNIFORM_PARTITION, limit2, DF_UNIFORM_PARTITION, NULL, NULL, integrals, DF_NO_HINT);
    if (status != DF_STATUS_OK)
    {
        return status + 0.4;
    }

    status = dfDeleteTask(&task);
    if (status != DF_STATUS_OK)
    {
        return status + 0.5;
    }

    return status;
}