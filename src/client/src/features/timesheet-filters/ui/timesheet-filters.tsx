import React from 'react';
import type { EmployeeType } from '@/entities/employee/model/employee.schema';
import type { ProjectType } from '@/entities/project/model/project.schema';

interface FilterProps {
  month: string;
  setMonth: (v: string) => void;
  employeeId: string;
  setEmployeeId: (v: string) => void;
  projectId: string;
  setProjectId: (v: string) => void;
  employees: EmployeeType[];
  projects: ProjectType[];
}

export const TimesheetFilters: React.FC<FilterProps> = ({
  month,
  setMonth,
  employeeId,
  setEmployeeId,
  projectId,
  setProjectId,
  employees,
  projects,
}) => (
  <div style={{ display: 'flex', gap: '15px', marginBottom: '20px', alignItems: 'flex-end' }}>
    <label style={{ display: 'flex', flexDirection: 'column', gap: '5px' }}>
      <span>Месяц:</span>
      <input type="month" value={month} onChange={(e) => setMonth(e.target.value)} />
    </label>

    <label style={{ display: 'flex', flexDirection: 'column', gap: '5px' }}>
      <span>Сотрудник:</span>
      <select value={employeeId} onChange={(e) => setEmployeeId(e.target.value)}>
        <option value="">Все сотрудники</option>
        {employees.map((e) => (
          <option key={e.id} value={e.id}>
            {e.name}
          </option>
        ))}
      </select>
    </label>

    <label style={{ display: 'flex', flexDirection: 'column', gap: '5px' }}>
      <span>Проект:</span>
      <select value={projectId} onChange={(e) => setProjectId(e.target.value)}>
        <option value="">Все проекты</option>
        {projects.map((p) => (
          <option key={p.id} value={p.id}>
            {p.number}
          </option>
        ))}
      </select>
    </label>
  </div>
);