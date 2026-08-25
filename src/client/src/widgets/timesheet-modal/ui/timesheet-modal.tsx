import { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { useMutation, useQueryClient } from '@tanstack/react-query';
import { 
  ZCreateTimeEntryForm, 
  ZUpdateTimeEntryForm, 
  type TimeEntryType 
} from '@/entities/time-entry/model/time-entry.schema';
import { createTimeEntry, updateTimeEntry } from '@/entities/time-entry/services/time-entry.service';
import type { EmployeeType } from '@/entities/employee/model/employee.schema';
import type { ProjectType } from '@/entities/project/model/project.schema';

interface Props {
  isOpen: boolean;
  onClose: () => void;
  editingItem?: TimeEntryType | null;
  employees: EmployeeType[];
  projects: ProjectType[];
}

export const TimesheetModal = ({ isOpen, onClose, editingItem, employees, projects }: Props) => {
  const queryClient = useQueryClient();
  const [serverError, setServerError] = useState<string | null>(null);

  const isEdit = Boolean(editingItem);

  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<any>({
    resolver: zodResolver(isEdit ? ZUpdateTimeEntryForm : ZCreateTimeEntryForm),
  });

  useEffect(() => {
    if (editingItem) {
      reset({
        id: editingItem.id,
        hours: editingItem.hours,
        comment: editingItem.comment || '',
        version: editingItem.version,
      });
    } else {
      reset({
        hours: 8,
        timesheetDate: new Date().toISOString().slice(0, 10),
        employeeId: '',
        projectId: '',
        comment: '',
      });
    }
    setServerError(null);
  }, [editingItem, isOpen, reset]);

  const mutation = useMutation({
    mutationFn: (data: any) => (isEdit ? updateTimeEntry(data) : createTimeEntry(data)),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['timeEntries'] });
      onClose();
    },
    onError: (err: any) => {
      const msg = err.response?.data?.detail || err.response?.data?.message || 'Ошибка сохранения';
      setServerError(msg);
    },
  });

  if (!isOpen) return null;

  return (
    <div style={{ position: 'fixed', inset: 0, background: 'rgba(0,0,0,0.5)', display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
      <div style={{ background: '#fff', padding: '20px', borderRadius: '8px', width: '400px' }}>
        <h3>{isEdit ? `Редактирование записи (${editingItem?.employeeName})` : 'Добавить запись'}</h3>

        {serverError && (
          <div style={{ background: '#f8d7da', color: '#721c24', padding: '10px', marginBottom: '10px', borderRadius: '4px' }}>
            {serverError}
          </div>
        )}

        <form onSubmit={handleSubmit((d) => mutation.mutate(d))} style={{ display: 'flex', flexDirection: 'column', gap: '10px' }}>
          
          {!isEdit && (
            <>
              <label>Дата:</label>
              <input type="date" {...register('timesheetDate')} />
              {errors.timesheetDate && <span style={{ color: 'red', fontSize: '12px' }}>{String(errors.timesheetDate.message)}</span>}

              <label>Сотрудник:</label>
              <select {...register('employeeId')}>
                <option value="">Выберите сотрудника</option>
                {employees.map((e) => (
                  <option key={e.id} value={e.id}>{e.name}</option>
                ))}
              </select>
              {errors.employeeId && <span style={{ color: 'red', fontSize: '12px' }}>{String(errors.employeeId.message)}</span>}

              <label>Проект:</label>
              <select {...register('projectId')}>
                <option value="">Выберите проект</option>
                {projects.map((p) => (
                  <option key={p.id} value={p.id}>{p.number}</option>
                ))}
              </select>
              {errors.projectId && <span style={{ color: 'red', fontSize: '12px' }}>{String(errors.projectId.message)}</span>}
            </>
          )}

          <label>Часы:</label>
          <input
            type="number"
            step="0.5"
            min="0.5"
            max="24"
            {...register('hours', { valueAsNumber: true })}
            />
          {errors.hours && <span style={{ color: 'red', fontSize: '12px' }}>{String(errors.hours.message)}</span>}

          <label>Комментарий:</label>
          <input type="text" {...register('comment')} />

          <div style={{ display: 'flex', gap: '10px', marginTop: '15px', justifyContent: 'flex-end' }}>
            <button type="button" onClick={onClose}>Отмена</button>
            <button type="submit" disabled={mutation.isPending}>
              {mutation.isPending ? 'Сохранение...' : 'Сохранить'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};