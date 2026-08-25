import React, { useState } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import type { TimeEntryType } from '@/entities/time-entry/model/time-entry.schema';
import { getTimeEntriesAll, deleteTimeEntry } from '@/entities/time-entry/services/time-entry.service';
import { getEmployeesAll } from '@/entities/employee/services/employee.service';
import { getProjectsAll } from '@/entities/project/services/project.service';

import { TimesheetFilters } from '@/features/timesheet-filters/ui/timesheet-filters';
import { TimesheetTable } from '@/widgets/timesheet-modal/ui/timesheet-table';
import { TimesheetModal } from '@/widgets/timesheet-modal/ui/timesheet-modal';

export const TimesheetPage: React.FC = () => {
  const queryClient = useQueryClient();
  const [month, setMonth] = useState('2026-03');
  const [employeeId, setEmployeeId] = useState('');
  const [projectId, setProjectId] = useState('');
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingItem, setEditingItem] = useState<TimeEntryType | null>(null);

  const { data: employees = [] } = useQuery({ queryKey: ['employees'], queryFn: getEmployeesAll });
  const { data: projects = [] } = useQuery({ queryKey: ['projects'], queryFn: getProjectsAll });

  const { data: entries = [], isLoading } = useQuery({
  queryKey: ['timeEntries', month, employeeId, projectId],
  queryFn: () =>
    getTimeEntriesAll({
      monthStr: month,
      employeeId,
      projectId,
    }),
});

  const deleteMutation = useMutation({
    mutationFn: deleteTimeEntry,
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['timeEntries'] }),
  });

  return (
    <div>
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
        <h2>ЭКРАН 1 — Табель учёта времени</h2>
        <button onClick={() => { setEditingItem(null); setIsModalOpen(true); }}>+ Добавить запись</button>
      </div>

      <TimesheetFilters
        month={month} setMonth={setMonth}
        employeeId={employeeId} setEmployeeId={setEmployeeId}
        projectId={projectId} setProjectId={setProjectId}
        employees={employees} projects={projects}
      />

      {isLoading ? <p>Загрузка данных...</p> : (
        <TimesheetTable
          entries={entries}
          onEdit={(item) => { setEditingItem(item); setIsModalOpen(true); }}
          onDelete={(id) => confirm('Удалить запись?') && deleteMutation.mutate(id)}
        />
      )}

      <TimesheetModal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        editingItem={editingItem}
        employees={employees}
        projects={projects}
      />
    </div>
  );
};