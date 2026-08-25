import { useState } from 'react';
import { useQuery } from '@tanstack/react-query';

import { getProjectReport } from '@/entities/project/services/project.service';
import { formatMoney } from '@/shared/lib/formatters';

export const ProjectReportPage = () => {
  const [month, setMonth] = useState('2026-03');

  const { data: report, isLoading } = useQuery({
    queryKey: ['projectReport', month],
    queryFn: () => getProjectReport(month),
  });

  const rows = report?.rows ?? [];

  return (
    <div>
      <h2>ЭКРАН 2 — Отчёт по проектам</h2>
      <div style={{ marginBottom: '20px' }}>
        <label>
          Выбор месяца:
          <input 
            type="month" 
            value={month} 
            onChange={(e) => setMonth(e.target.value)} 
            style={{ marginLeft: '10px' }} 
          />
        </label>
      </div>

      {isLoading ? (
        <p>Загрузка отчёта...</p>
      ) : (
        <table border={1} cellPadding={8} style={{ width: '100%', borderCollapse: 'collapse' }}>
          <thead>
            <tr>
              <th>Номер</th>
              <th>Проект</th>
              <th>Часы</th>
              <th>Стоимость</th>
              <th>Бюджет</th>
              <th>% Освоения</th>
            </tr>
          </thead>
          <tbody>
            {rows.map((r) => {
              // Подсветка фона: розоватая при перерасходе (>100%), желтоватая при риске (80-100%)
              const rowBg = r.isOverrun ? '#ffe6e6' : r.isRisk ? '#fffbe6' : 'transparent';
              const statusColor = r.isOverrun ? 'red' : r.isRisk ? 'orange' : 'green';

              return (
                <tr key={r.projectId} style={{ background: rowBg }}>
                  <td>{r.projectNumber}</td>
                  <td>{r.projectName}</td>
                  <td>{r.totalHours} ч.</td>
                  <td>{formatMoney(r.totalCost)}</td>
                  <td>{formatMoney(r.budget)}</td>
                  <td style={{ color: statusColor, fontWeight: 'bold' }}>
                    {r.budgetUsagePercentage}% 
                    {r.isOverrun && ' (Перерасход!)'}
                    {r.isRisk && ' (Риск)'}
                  </td>
                </tr>
              );
            })}
          </tbody>
          <tfoot>
            <tr style={{ fontWeight: 'bold', background: '#f0f0f0' }}>
              <td colSpan={2}>ИТОГО:</td>
              <td>{report?.grandTotalHours ?? 0} ч.</td>
              <td>{formatMoney(report?.grandTotalCost ?? 0)}</td>
              <td>{formatMoney(report?.grandTotalBudget ?? 0)}</td>
              <td style={{ 
                color: report?.isGrandOverrun ? 'red' : report?.isGrandRisk ? 'orange' : 'black' 
              }}>
                {report?.grandBudgetUsagePercentage ?? 0}%
              </td>
            </tr>
          </tfoot>
        </table>
      )}
    </div>
  );
};