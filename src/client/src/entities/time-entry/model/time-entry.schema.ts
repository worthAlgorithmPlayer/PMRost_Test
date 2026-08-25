import { z } from 'zod';

export const ZTimeEntry = z.object({
  id: z.string(),
  employeeName: z.string(),
  projectNumber: z.string(),
  timeSheetDate: z.string(),
  hours: z.number(),
  rate: z.number(),
  price: z.number(),
  version: z.number(),
  
  employeeId: z.string().optional(),
  projectId: z.string().optional(),
  projectName: z.string().optional(),
  comment: z.string().nullable().optional(),
  isOvertime: z.boolean().optional().default(false),
});

export const ZTimeEntriesResponse = z.object({
  items: z.array(ZTimeEntry),
  totalCount: z.number(),
});

export const ZTimeEntryForm = z.object({
  id: z.string().optional(),
  employeeId: z.string().min(1, 'Выберите сотрудника'),
  projectId: z.string().min(1, 'Выберите проект'),
  timesheetDate: z.string().regex(/^\d{4}-\d{2}-\d{2}$/, 'Укажите дату YYYY-MM-DD'),
  hours: z.number().int().positive('Часы должны быть положительным числом'), 
  comment: z.string().nullable().optional(),
  version: z.number().optional(),
});

export const ZTimeEntryFilter = z
  .object({
    monthStr: z.string().optional(),
    employeeId: z.string().optional(),
    projectId: z.string().optional(),
    skip: z.number().default(0),
    limit: z.number().default(10),
  })
  .transform((data) => {
    let year: number | undefined;
    let month: number | undefined;

    if (data.monthStr) {
      const [y, m] = data.monthStr.split('-');
      if (y && m) {
        year = parseInt(y, 10);
        month = parseInt(m, 10);
      }
    }

    return {
      Year: year,
      Month: month,
      EmployeeId: data.employeeId || undefined,
      ProjectId: data.projectId || undefined,
      Skip: data.skip,
      Limit: data.limit,
    };
  });

  const baseFormFields = {
  hours: z.union([z.string(), z.number()]),
  comment: z.string().nullable().optional(),
};

export const ZCreateTimeEntryForm = z.object({
  ...baseFormFields,
  employeeId: z.string().min(1, 'Выберите сотрудника'),
  projectId: z.string().min(1, 'Выберите проект'),
  timesheetDate: z.string().regex(/^\d{4}-\d{2}-\d{2}$/, 'Укажите дату YYYY-MM-DD'),
});

export const ZUpdateTimeEntryForm = z.object({
  ...baseFormFields,
  id: z.string(),
  version: z.number(),
});

export type TimeEntryType = z.infer<typeof ZTimeEntry>;
export type TimeEntryFormType = z.infer<typeof ZTimeEntryForm>;

export type TimeEntryFilterInputType = z.input<typeof ZTimeEntryFilter>;
export type TimeEntryFilterType = z.output<typeof ZTimeEntryFilter>;

export type CreateTimeEntryFormType = z.infer<typeof ZCreateTimeEntryForm>;
export type UpdateTimeEntryFormType = z.infer<typeof ZUpdateTimeEntryForm>;
